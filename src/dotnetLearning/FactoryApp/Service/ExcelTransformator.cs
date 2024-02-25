using dotnetLearning.FactoryApp.Model;
using dotnetLearning.FactoryApp.Service.FacilityService;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

namespace dotnetLearning.FactoryApp.Service
{
    public class ExcelTransformator(IOptions<FacilityServiceOptions> options)
    {
        public async Task WriteToExcelAsync(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks)
        {
            if (string.IsNullOrEmpty(options.Value.FacilitiesExcelFilePath)) throw new ArgumentNullException("Неверно задан путь файла Excel");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            await Task.Run(() =>
            {
                using var package = new ExcelPackage();
                var worksheetFactories = package.Workbook.Worksheets.Add("Factories");
                var worksheetUnits = package.Workbook.Worksheets.Add("Units");
                var worksheetTanks = package.Workbook.Worksheets.Add("Tanks");

                worksheetFactories.Cells["A1"].Value = "Id";
                worksheetFactories.Cells["B1"].Value = "Name";
                worksheetFactories.Cells["C1"].Value = "Description";

                worksheetUnits.Cells["A1"].Value = "Id";
                worksheetUnits.Cells["B1"].Value = "Name";
                worksheetUnits.Cells["C1"].Value = "Description";
                worksheetUnits.Cells["D1"].Value = "FacyoryId";

                worksheetTanks.Cells["A1"].Value = "Id";
                worksheetTanks.Cells["B1"].Value = "Name";
                worksheetTanks.Cells["C1"].Value = "Description";
                worksheetTanks.Cells["D1"].Value = "UnitId";
                worksheetTanks.Cells["E1"].Value = "Volume";
                worksheetTanks.Cells["F1"].Value = "MaxVolume";

                int row = 2;
                foreach (var item in factories)
                {
                    worksheetFactories.Cells[row, 1].Value = item.Id;
                    worksheetFactories.Cells[row, 2].Value = item.Name;
                    worksheetFactories.Cells[row, 3].Value = item.Description;
                    row++;
                }

                row = 2;
                foreach (var item in units)
                {
                    worksheetUnits.Cells[row, 1].Value = item.Id;
                    worksheetUnits.Cells[row, 2].Value = item.Name;
                    worksheetUnits.Cells[row, 3].Value = item.Description;
                    worksheetUnits.Cells[row, 4].Value = item.FactoryId;
                    row++;
                }

                row = 2;
                foreach (var item in tanks)
                {
                    worksheetTanks.Cells[row, 1].Value = item.Id;
                    worksheetTanks.Cells[row, 2].Value = item.Name;
                    worksheetTanks.Cells[row, 3].Value = item.Description;
                    worksheetTanks.Cells[row, 4].Value = item.UnitId;
                    worksheetTanks.Cells[row, 5].Value = item.Volume;
                    worksheetTanks.Cells[row, 6].Value = item.MaxVolume;
                    row++;
                }

                foreach (var sh in package.Workbook.Worksheets)
                    sh.Cells[sh.Dimension.Address].AutoFitColumns();

                try
                {
                    package.SaveAs(new FileInfo(options.Value.FacilitiesExcelFilePath));
                }
                catch (InvalidOperationException)
                {
                    package.SaveAs(new FileInfo($"{options.Value.FacilitiesExcelFilePath} Reserved.xlsx"));
                }
            });
        }
    }
}