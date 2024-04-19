using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using test.Model;

namespace test.Service
{
    public class Util
    {
        public List<Data> ReadExcel(string filePath)
        {
            var employeeDataList = new List<Data>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var employeeData = new Data
                    {
                        MemberID = worksheet.Cells[row, 1].Value?.ToString(),
                        Name = worksheet.Cells[row, 2].Value?.ToString(),
                        Level = int.Parse(worksheet.Cells[row, 3].Value?.ToString()),
                        SV = decimal.Parse(worksheet.Cells[row, 4].Value?.ToString()),
                        Percentage = int.Parse(worksheet.Cells[row, 5].Value?.ToString()),
                        Points = decimal.Parse(worksheet.Cells[row, 6].Value?.ToString()),
                        SVSource = worksheet.Cells[row, 7].Value?.ToString()
                    };

                    employeeDataList.Add(employeeData);
                }
            }

            return employeeDataList;
        }


        public string GenerateInsertQuery(List<Data> DataList)
        {
            var stringBuilder = new System.Text.StringBuilder();

            stringBuilder.AppendLine("INSERT INTO tbl_TempLeadershipBonus_PH30000696 (MemberID, Name, Level, SV, Percentage, Points, SV_SOURCE) VALUES");

            foreach (var employeeData in DataList)
            {
                stringBuilder.AppendLine($"('{employeeData.MemberID}', '{employeeData.Name}', {employeeData.Level}, {employeeData.SV}, {employeeData.Percentage}, {employeeData.Points}, '{employeeData.SVSource}'),");
            }

            stringBuilder.Length -= 3;

            return stringBuilder.ToString();
        }
    }
}
