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
        public async Task WriteToExcelAsync(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks)
        {
            await Task.Run(() =>
            {
                using var package = new ExcelPackage();

                WriteWorksheet(factories, package.Workbook.Worksheets.Add("Factories"), GetFactoryHeaders());
                WriteWorksheet(units, package.Workbook.Worksheets.Add("Units"), GetUnitHeaders());
                WriteWorksheet(tanks, package.Workbook.Worksheets.Add("Tanks"), GetTankHeaders());

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

        private static void WriteWorksheet<T>(IEnumerable<T> items, ExcelWorksheet worksheet, string[] headers, Dictionary<string, Func<T, object>> properiesSelectors)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            foreach (var item in items)
            {

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[row, i + 1].Value 
                }

                //#nullable disable
                //                var properties = item.GetType().GetProperties();
                //#nullable restore

                //                for (int i = 0; i < headers.Length; i++)
                //                {
                //                    worksheet.Cells[row, i + 1].Value = properties.First(p => p.Name == headers[i]).GetValue(item);
                //                }
                //                row++;
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

        private static IList<T> ReadFromExcel<T>(string excelFilePath, string sheetName)
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

                var properties = typeof(T).GetProperties();
                var numberOfColumns = properties.Length;

                var startColumn = 1;
                for (int col = 1; col <= numberOfColumns; col++)
                {
                    var cellValue = worksheet.Cells[row, startColumn + col - 1].Value;

                    if (cellValue is null)
                        continue;

                    if (cellValue is double)
                        cellValue = Convert.ToInt32(cellValue);

                    properties[col - 1].SetValue(item, cellValue);
                }
                items.Add(item);
            }
            return items;
        }

        private string[] GetFactoryHeaders()
        {
            return ["Id", "Name", "Description"];
        }

        private string[] GetUnitHeaders()
        {
            return ["Id", "Name", "Description", "FactoryId"];
        }

        private string[] GetTankHeaders()
        {
            return ["Id", "Name", "Description", "UnitId", "Volume", "MaxVolume"];
        }
    }
}