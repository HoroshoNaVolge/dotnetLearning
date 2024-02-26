using dotnetLearning.FactoryApp.Model;
using dotnetLearning.FactoryApp.Service.FacilityService;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

namespace dotnetLearning.FactoryApp.Service
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
                catch (InvalidOperationException)
                {
                    package.SaveAs(new FileInfo($"{excelFilePath} Reserved.xlsx"));
                }
            });
        }

        private static void WriteWorksheet<T>(IEnumerable<T> items, ExcelWorksheet worksheet, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            foreach (var item in items)
            {
#nullable disable
                var properties = item.GetType().GetProperties();
#nullable restore

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[row, i + 1].Value = properties.First(p => p.Name == headers[i]).GetValue(item);
                }
                row++;
            }
        }

        public async Task GetFacilitiesFromExcel()
        {
            await Task.Run(() =>
            {
                ReadFromExcel<Factory>(excelFilePath, "Factories");
                ReadFromExcel<Unit>(excelFilePath, "Units");
                ReadFromExcel<Tank>(excelFilePath, "Tanks");
            });
        }

        private static List<T> ReadFromExcel<T>(string excelFilePath, string sheetName)
        {
            List<T> items = [];

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

                    // читаем строки
                    try
                    {
                        properties[col - 1].SetValue(item, cellValue);
                    }

                    // потому что читает число как double
                    catch (ArgumentException)
                    {
                        properties[col - 1].SetValue(item, Convert.ToInt32(cellValue));
                    }
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