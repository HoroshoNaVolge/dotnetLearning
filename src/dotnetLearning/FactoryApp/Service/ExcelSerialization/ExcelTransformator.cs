using dotnetLearning.FactoryApp.Model;
using dotnetLearning.FactoryApp.Service.FacilityService;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

namespace dotnetLearning.FactoryApp.Service.ExcelSerialization
{
    public class ExcelTransformator
    {
        private readonly string excelFilePath;
        public ExcelTransformator(IOptions<FacilityServiceOptions> options)
        {
            if (string.IsNullOrEmpty(options.Value.FacilitiesExcelFilePath)) throw new ArgumentNullException("Неверно задан путь файла Excel");

            excelFilePath = options.Value.FacilitiesExcelFilePath;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public async Task WriteToExcelAsync(IList<Factory> factories, IList<Unit> units, IList<Tank> tanks)
        {
            var factoryValues = factories.Select(Factory.GetValues).ToList();
            var unitValues = units.Select(Unit.GetValues).ToList();
            var tankValues = tanks.Select(Tank.GetValues).ToList();

            await Task.Run(() =>
            {
                using var package = new ExcelPackage();

                WriteWorksheet(factoryValues, Factory.GetFactoryKeys(), package.Workbook.Worksheets.Add("Factories"));
                WriteWorksheet(unitValues, Unit.GetUnitKeys(), package.Workbook.Worksheets.Add("Units"));
                WriteWorksheet(tankValues, Tank.GetTankKeys(), package.Workbook.Worksheets.Add("Tanks"));

                foreach (var sh in package.Workbook.Worksheets)
                    sh.Cells[sh.Dimension.Address].AutoFitColumns();

                try
                {
                    package.SaveAs(new FileInfo(excelFilePath));
                }
                // Если файл уже открыт в excel например, то сохраняем с другим именем.
                // Проверить без исключения не понимаю как, даже через поток FileStream нужно ловить будет.
                catch (InvalidOperationException)
                {
                    package.SaveAs(new FileInfo($"{excelFilePath} Reserved.xlsx"));
                }
            });
        }

        private static void WriteWorksheet(IList<IDictionary<string, object?>> values, IList<string> keys, ExcelWorksheet worksheet)
        {
            for (int i = 0; i < keys.Count; i++)
                worksheet.Cells[1, i + 1].Value = keys[i];

            int row = 2;
            foreach (var value in values)
            {
                for (int i = 0; i < value.Count; i++)
                    worksheet.Cells[row, i + 1].Value = value[keys[i]];
                row++;
            }
        }

        public async Task<FacilitiesContainer> GetFacilitiesFromExcel(FacilitiesContainer container)
        {
            await Task.Run(() =>
            {
                container.Factories = ReadFromExcel<Factory>(excelFilePath, "Factories");
                container.Units = ReadFromExcel<Unit>(excelFilePath, "Units");
                container.Tanks = ReadFromExcel<Tank>(excelFilePath, "Tanks");
            });

            return container;
        }

        // это было жоска
        private static IList<T> ReadFromExcel<T>(string excelFilePath, string sheetName) where T : IEntity<T>
        {
            IList<T> items = [];

            using var package = new ExcelPackage(new FileInfo(excelFilePath));
            var worksheet = package.Workbook.Worksheets[sheetName];

            if (worksheet is null)
                return items;

            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                T item = Activator.CreateInstance<T>();

                var values = item.GetValues();
                var keys = item.GetKeys();

                for (int col = 1; col <= keys.Count; col++)
                {
                    var cellValue = worksheet.Cells[row, col].Value;

                    if (cellValue is null)
                        continue;

                    if (cellValue is double)
                        cellValue = Convert.ToInt32(cellValue);

                    values[keys[col - 1]] = cellValue;
                }
                items.Add(item.Create(values));
            }
            return items;
        }
    }
}