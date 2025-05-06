using Microsoft.EntityFrameworkCore;
using Standus_5_0.Data;
using OfficeOpenXml;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Xml.Serialization;
namespace Standus_5_0.Areas.Report.Models
{

    public class ExcelImport
    {
        private readonly ApplicationDbContext _context;
        public ExcelImport(ApplicationDbContext context)
        {
            _context = context;
        }
        //public int copyData(int reportid, int id)
        //{
        //    try
        //    {                        

        //        var report = (_context.Reports.Where(f => f.ReportID == reportid));

        //        var excolumns = _context.ExcelColumns;
        //        int rreportid = report.First().ReportID;
        //        var excols = excolumns.Where(ec => ec.ReportID == rreportid);

        //        string values;
        //        int count;
        //        count = excols.Count();
        //        string sql = "";

        //        //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; 

        //        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        //        using (var stream = new MemoryStream())
        //        {
        //            //await model.FileUpload.CopyToAsync(stream);
        //            using (var package = new ExcelPackage(stream))
        //            {
        //                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        //                if (worksheet == null)
        //                {
        //                    //ModelState.AddModelError("", "No worksheet found in Excel file.");
        //                    return 0;
        //                }
        //                int rowCount = worksheet.Dimension.Rows;

        //                for (int row = 2; row <= rowCount; row++)
        //                {
        //                    values = " values (";
        //                    for (int i = 0; i <= count - 1; i++)
        //                        values = values + "'" + worksheet.Cells[row, 1].Text.Trim() + "',";
        //                        values = values + "'" + DateTime.Now + "'";
        //                        values = values + "," + id + ")";
        //                        sql = "insert into " + report.First().Ref_Table + values;
        //                        _context.Database.ExecuteSqlRaw(sql);
        //                }
        //            }
        //        }

        //        //foreach (System.Data.DataRow row in DtSet.Tables[0].Rows)
        //        //{
        //        //    values = " values (";
        //        //    for (int i = 0; i <= count - 1; i++)
        //        //        values = values + "'" + row[i] + "',";
        //        //    values = values + "'" + DateTime.Now + "'";
        //        //    values = values + "," + id + ")";
        //        //    sql = "insert into " + report.First().Ref_Table + values;
        //        //    _context.Database.ExecuteSqlRaw (sql);
        //        //}

        //        //conexcel.Close();
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        public int CopyData(int reportId, int userId)
        {
            try
            {
                // Load report metadata
                var report = _context.Reports.FirstOrDefault(f => f.ReportID == reportId);
                if (report == null)
                    return 0;

                // Build OleDb connection string
                string connectionString =
                    $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={report.FilePath};Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'";

                using var connection = new System.Data.OleDb.OleDbConnection(connectionString);
                connection.Open();

                // Load Excel data
                var command = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [Sheet1$]", connection);
                var dataSet = new DataSet();
                command.Fill(dataSet);

                connection.Close();

                // Load Excel column mappings
                //using var exColumnsContext = new ExcelColumnsContext("ERP");
                var excelColumns = _context.ExcelColumns
                    .Where(e => e.ReportID == report.ReportID)
                    .ToList();

                int columnCount = excelColumns.Count;

                //using var db = new YourDbContext("ERP"); // <-- Replace with your EF Core DbContext class name

                // Optionally clear previous data
                // await db.Database.ExecuteSqlRawAsync($"DELETE FROM {report.Ref_Table}");

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var valuesBuilder = new StringBuilder();
                    valuesBuilder.Append(" VALUES (");

                    // Add each cell as quoted string
                    for (int i = 0; i < columnCount; i++)
                    {
                        string value = row[i]?.ToString().Replace("'", "''"); // escape quotes
                        valuesBuilder.Append($"'{value}',");
                    }

                    // Append DateTime and userId
                    valuesBuilder.Append($"'{DateTime.Now}',{userId})");

                    string sql = $"INSERT INTO {report.Ref_Table}{valuesBuilder}";
                    _context.Database.ExecuteSqlRaw(sql);
                }

                return 1;
            }
            catch (Exception ex)
            {
                // log ex if needed
                return 0;
            }
        }

    }

}
