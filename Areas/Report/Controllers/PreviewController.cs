using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.Report.Models;
using Standus_5_0.Data;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Data.SqlClient;
using Standus_5_0.Areas.Report.Data;
using Microsoft.AspNetCore.Authorization;

namespace Standus_5_0.Areas.Report.Controllers
{
    // Imports Excel = Microsoft.Office.Interop.Excel
    [Area("Report")]
    [Authorize]
    public class PreviewController : Controller
        {
            private readonly ApplicationDbContext _context;
            private readonly PdfService _pdfService;
            private readonly ExcelImport _excelImport;
            public PreviewController(ApplicationDbContext context,PdfService pdfService, ExcelImport excelImport)
            {
                _context = context;
                _pdfService = pdfService;
                _excelImport = excelImport;
            }
        //private ReportFilterContext rFilters = new ReportFilterContext("ERP");
        // GET: Preview
        //public ActionResult Index(int reportid, string ReportName, FormCollection? cntr)
        //{
        //    ViewData["ID"] = reportid;
        //    ViewData["ReportName"] = ReportName;
        //    ViewData["IsPreview"] = "true";

        //    List<FilterValues> filters = new List<FilterValues>();

        //    var rFilters = _context.FilterValues;

        //    foreach (var ct in cntr)
        //    {
        //        var flt = from f in _context.ReportFilters
        //                  where f.ReportID == reportid && f.Name == ct.Value.ToString()
        //                  select f;
        //        filters.Add(new FilterValues() { Name = ct.Value.ToString(), Value = ct.Value.ToString(), 
        //            DefaultValue = flt.First().DefaultValue, type = flt.First().Type });
        //    }

        //    ViewBag.Filters = filters;

        //    var report = _context.Reports.Where(rep => rep.ReportID == reportid);

        //    var filts = _context.ReportFilters.Where(filt => filt.ReportID == reportid);

        //    var cols = _context.ReportColumns
        //    .Where(col => col.ReportID == reportid && col.HideColumn == false)
        //        .OrderBy(f => f.ColumnNo);

        //    var dchart = _context.ReportCharts.Where(cht => cht.ReportID == reportid);

        //var group = _context.Groups.OrderBy(g => g.Groupname);

        //    var preview = new ReportPreview();
        //    preview.Report  = report.First();
        //    preview.Filter = filts.ToList();
        //    preview.Column  = cols.ToList();
        //    preview.Chart = dchart.ToList();
        //    ViewData["Group"] = group;
        //    return View(preview);
        //}
        [HttpGet]
        public ActionResult Index(int reportid, string ReportName)
        {
            ViewData["ID"] = reportid;
            ViewData["ReportName"] = ReportName;
            ViewData["IsPreview"] = "true";



            //var rFilters = _context.FilterValues;


            //var filters = _context.ReportFilters
            //          .Where(f => f.ReportID == reportid);


            //ViewBag.Filters = filters;

            var report = _context.Reports.Where(rep => rep.ReportID == reportid);

            var filts = _context.ReportFilters.Where(filt => filt.ReportID == reportid);

            var cols = _context.ReportColumns
            .Where(col => col.ReportID == reportid && col.HideColumn == false)
                .OrderBy(f => f.ColumnNo);

            var dchart = _context.ReportCharts.Where(cht => cht.ReportID == reportid);
            var subquery = _context.ReportsSubquery.Where(subq => subq.ReportID == reportid);
            var group = _context.Groups.OrderBy(g => g.Groupname);
            var excelcolumns = _context.ExcelColumns.Where(e => e.ReportID == reportid);

            var preview = new ReportPreview();
            preview.Report = report.First();
            preview.Filter = filts.ToList();
            preview.Column = cols.ToList();
            preview.Chart = dchart.ToList();
            preview.subquery = subquery.ToList();
            preview.excelcolumns = excelcolumns.ToList();
            preview.tables = new List<string>();

            var dbSetProperties = typeof(ApplicationDbContext).GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(p => p.Name)
            .ToList();

                    foreach (var table in dbSetProperties)
                    {
                        preview.tables.Add(table); 
                    }

            ViewData["Group"] = group;
            ViewData["conn"] = _context.Database.GetDbConnection().ConnectionString;
            return View(preview);
        }

        [HttpPost]
        public ActionResult Index(int reportid,string ReportName,IFormCollection form)
        {
            ViewData["ID"] = reportid;
            ViewData["ReportName"] = ReportName;
            ViewData["IsPreview"] = "true";

            //int reportid = Convert.ToInt16(form["reportid"]);

            //var rFilters = _context.FilterValues;


            //var filters = _context.ReportFilters
            //          .Where(f => f.ReportID == reportid);

            List<FilterValues> filters = new List<FilterValues>();
            foreach (var key in form.Keys)
            {
                if (key != "reportid" && key != "ReportName") { 
                    var flt = from f in _context.ReportFilters
                              where f.ReportID == reportid & f.Name == key
                              select f;
                    filters.Add(new FilterValues()
                    {
                        Name = key,
                        Value = form[key],
                        DefaultValue = flt.First().DefaultValue,
                        type = flt.First().Type
                    });
                }
            }

            ViewBag.Filters = filters;

            //ViewBag.Filters = filters;

            var report = _context.Reports.Where(rep => rep.ReportID == reportid);

            var filts = _context.ReportFilters.Where(filt => filt.ReportID == reportid);

            var cols = _context.ReportColumns
            .Where(col => col.ReportID == reportid && col.HideColumn == false)
                .OrderBy(f => f.ColumnNo);

            var dchart = _context.ReportCharts.Where(cht => cht.ReportID == reportid);
            var subquery = _context.ReportsSubquery.Where(subq => subq.ReportID == reportid);
            var group = _context.Groups.OrderBy(g => g.Groupname);
            var excelcolumns = _context.ExcelColumns.Where(e => e.ReportID == reportid);

            var preview = new ReportPreview();
            preview.Report = report.First();
            preview.Filter = filts.ToList();
            preview.Column = cols.ToList();
            preview.Chart = dchart.ToList();
            preview.subquery = subquery.ToList();
            preview.excelcolumns = excelcolumns.ToList();
            preview.tables = new List<string>();

            var dbSetProperties = typeof(ApplicationDbContext).GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(p => p.Name)
            .ToList();

            foreach (var table in dbSetProperties)
            {
                preview.tables.Add(table);
            }

            ViewData["Group"] = group;
            ViewData["conn"] = _context.Database.GetDbConnection().ConnectionString;
            return View(preview);
        }
        public ActionResult list(string userid)
            {
            /* add this to loggin screen once done
             * 
             * HttpContext.Session.SetString("superuser", "true");
                HttpContext.Session.SetString("admin", "true");
             */

            //var users = new UserRightsContext("ERP")
            //Dim reprights = (From rp In rep.ToList
            //                 Join right As UserRights In users.Rights.ToList On rp.id Equals CInt(right.Report)
            //                 Select New With {
            //                  .Group = rp.Group,
            //                  .type = rp.type,
            //                  .name = rp.name,
            //                  .id = rp.id,
            //                  .userid = right.UserID,
            //                  .date_created = rp.date_created,
            //                  .date_updated = rp.date_updated
            //                 }).Where(Function(f) f.userid = Session("userid")).OrderBy(Function(f) f.Group)
                     
                           

            ViewBag.IsSuperUser = HttpContext.Session.GetString("superuser") == "true";
            ViewBag.IsAdmin = HttpContext.Session.GetString("admin") == "true";

            var rep = (from rps in _context.Reports.ToList()
                           join gp in _context.Groups.ToList() on rps.GroupID equals gp.GroupID
                           select new
                           {
                               Group = gp.Groupname,
                               type = rps.ReportType,
                               name = rps.ReportName,
                               id = rps.ReportID,
                               date_created = rps.Date_Created,
                               date_updated = rps.Date_updated,
                               hide = rps.Hide
                           }).Where(f => f.hide == false).OrderBy(f => f.Group);

            string userID = HttpContext.Session.GetString("userid");
            var reprights = (from rp in rep.ToList()
                             join right in _context.ApplicationUser.ToList() on rp.id.ToString()  equals right.Report
                             select new
                             {
                                 Group = rp.Group,
                                 type = rp.type,
                                 name = rp.name,
                                 id = rp.id,
                                 userid = right.Id,
                                 date_created = rp.date_created,
                                 date_updated = rp.date_updated
                             }).Where(f => f.userid.ToString() == userID).OrderBy(f => f.Group);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            List<Reports> repos = null ;

            if (User.IsInRole("Superadmin")) {
                repos = (from repo in _context.Reports
                         select repo)
                            .Include(r => r.group)
                            .ToList();
            }
            else
            {
                repos = (from repo in _context.Reports
                            join clm in _context.AccessClaims on repo.ReportID equals clm.ReportId
                         where clm.UserId == userId
                            select repo)
                            .Include(r => r.group)
                            .ToList();

                
            }

            ViewData["Report"] = repos;
            ViewData["Group"] = _context.Groups.OrderBy(f => f.Groupname);
            ViewData["reprights"] = reprights;
            //ViewData["Report"] = _context.Reports.Include(r => r.group);



            return View();
            }

            // GET: Preview/Details/5
            public JsonResult chartdata(int reportID, int chartid, string sql)
            {
                var charts = _context.ReportCharts;

                var reports = _context.Reports;

                var cht = from ch in charts
                          where ch.ReportID == reportID && ch.Id == chartid
                          select ch;
                var report = from rpt in reports
                             where rpt.ReportID == reportID
                             select rpt;

                string CS = _context.Database.GetDbConnection().ConnectionString;

                System.Data.DataTable dtt = new System.Data.DataTable();
                SqlConnection  conn = new SqlConnection(CS);
                System.Data.IDataReader drr;
                SqlCommand cmd = new SqlCommand();

                // Dim values As String() = sql.Split("$$")
                // Dim query = sql.Split("$$").ElementAt(0)
                var xvalue = cht.First().XValue;
                var yovalue = cht.First().YOValue;
                var ytvalue = cht.First().YTValue;

                // Dim query = cht.First.
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql; //report.First().SqlQuery;
                conn.Open();
                drr = cmd.ExecuteReader(); 

                dtt.Load(drr);

                xvalue = xvalue.Replace("[", "");
                xvalue = xvalue.Replace("]", "");

                ytvalue = ytvalue.Replace("[", "");
                ytvalue = ytvalue.Replace("]", "");

                yovalue = yovalue.Replace("[", "");
                yovalue = yovalue.Replace("]", "");

                List<object> iData = new List<object>();
                DataTable dt = new DataTable();

                dt.Columns.Add("'" + xvalue + "'", typeof(string));
                dt.Columns.Add("'" + yovalue + "'", typeof(string));
                dt.Columns.Add("'" + ytvalue + "'", typeof(string));
                foreach (DataRow tk in dtt.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["'" + xvalue + "'"] = tk[xvalue];
                    dr["'" + yovalue + "'"] = tk[yovalue];
                    dr["'" + ytvalue + "'"] = 0;
                    dt.Rows.Add(dr);
                }

                foreach (DataColumn dc in dt.Columns)
                {
                    var x = dt.AsEnumerable()
                      .Select(row => row[dc.ColumnName])
                      .ToList();
                    iData.Add(x);
                }

                return Json(iData);
            }

            [HttpPost]
            //[ValidateInput(false)]
            public IActionResult ToPDF(string report_topdf, string chart_toPdf, string filename)
            {
                string fname = filename;
                string outputpath = GeneratePDF(report_topdf, chart_toPdf, fname);
                // Set the content type And file name for the response
                //string ContentType = "application/vnd.ms-excel";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=TableData.pdf");

                // Convert the Excel XML content to bytes
                //byte[] fileBytes = Encoding.UTF8.GetBytes(report_topdf);

                // Send the Excel XML as a download
                return PhysicalFile(outputpath, "application/pdf", fname);
            }
            public string GeneratePDF(string report_topdf, string chart_toPdf, string filename)
            {
               string outputPath = _pdfService.GeneratePDF(report_topdf,chart_toPdf,filename);    

               return outputPath;
            }

            // GET: Preview/Create
            [HttpPost]
            [IgnoreAntiforgeryToken]
            public FileContentResult ToExcel([FromForm] string report_data_control)
            {
                if (!string.IsNullOrWhiteSpace(report_data_control))
                {
                    report_data_control = report_data_control.Replace(Environment.NewLine, "");
                }

                string excelXml = GenerateExcelXml(report_data_control);
                byte[] fileBytes = Encoding.UTF8.GetBytes(excelXml);

                return File(fileBytes, "application/vnd.ms-excel", "TableData.xls");
            }


            public ActionResult ExcelProcess(int reportID)
            {
                //var db = _context.ExcelCustomFields.Where(cf => cf.ReportID == reportID);
                var db = _context.Reports
                .Include(r => r.ExcelColumns)
                .Include(r => r.CustomFields)
                .Where(r => r.ReportID == reportID);
                return View(db.ToList());
            }

        //public ActionResult dataimport(ExcelCustomFields ex)
        [HttpPost]
        public ActionResult dataimport(int reportid)
        {
                string sql = "";

                sql = "insert into ExcelProcess(ReportID,ProcessDate) values(" + reportid + ",'" + DateTime.Now + "')";
                sql = sql + " select SCOPE_IDENTITY() As ID ";

                // Dim dbcon As New DbContext("ERP")
                // dim id As Integer=dbcon.Database.ExecuteSqlCommand(sql)

                string CS = _context.Database.GetDbConnection().ConnectionString;
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection conn = new SqlConnection(CS);
                System.Data.IDataReader dr;
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                cmd.CommandText = sql;
                int id = cmd.ExecuteNonQuery();
                // conn.close
                //foreach (var cv in ex.CustomValues)
                //{
                //    cv.ID = id;
                //    // Dim db As New ExcelCustomValuesContext ("ERP")
                //    // db.CustomValues.Add(cv)
                //    // db.SaveChanges  
                //    sql = "insert into ReportsExcelCustomValues values(" + cv.ID + "," + cv.CustomFieldID + ",'" + cv.CustomValue + "')";
                //    cmd.CommandText = sql;
                //    cmd.ExecuteNonQuery();
                //}

                conn.Close();

                //ExcelImport ei = new ExcelImport();
                var success = _excelImport.CopyData(reportid, id);

                RouteValueDictionary val = new RouteValueDictionary();
                val.Add("reportid", reportid);

                return RedirectToAction("ExcelProcess", val);
            }
            private string GenerateExcelXml(string tableData)
            {
                tableData = tableData.Replace("                ", "");
                tableData = tableData.Replace("            ", "");
                tableData = tableData.Replace("        ", "");
                string[] tbody = Regex.Split(tableData, "<tbody>(.*?)</tbody>", RegexOptions.IgnoreCase);
                string[] tr = Regex.Split(tbody[1], "<tr>(.*?)</tr>", RegexOptions.IgnoreCase);

                StringBuilder excelXml = new StringBuilder();
                int i;
                int j;
                string rowtext;

                // Excel XML header
                excelXml.AppendLine("<?xml version=\"1.0\"?>");
                excelXml.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
                excelXml.AppendLine("<ss:Workbook xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                excelXml.AppendLine("   <ss:Styles>");
                excelXml.AppendLine("      <!-- Define a style for bold text -->");
                excelXml.AppendLine("      <ss:Style ss:ID=\"BoldText\">");
                excelXml.AppendLine("         <ss:Font ss:Bold=\"1\"/>");
                excelXml.AppendLine("         <ss:Borders>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("         </ss:Borders>");
                excelXml.AppendLine("      </ss:Style>");
                excelXml.AppendLine("      <!-- Define a style for border lines -->");
                excelXml.AppendLine("      <ss:Style ss:ID=\"BorderedCell\">");
                excelXml.AppendLine("         <ss:Borders>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("            <ss:Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                excelXml.AppendLine("         </ss:Borders>");
                excelXml.AppendLine("      </ss:Style>");
                excelXml.AppendLine("   </ss:Styles>");
                excelXml.AppendLine("   <ss:Worksheet ss:Name=\"Table Data\">");
                excelXml.AppendLine("      <ss:Table>");

                // Loop through your model data and populate Excel XML
                // For Each item In model
                for (i = 3; i <= tr.Length - 1; i++)
                {
                    rowtext = tr[i];
                    if (rowtext != "")
                    {
                        string[] td;
                        if (i == 3)
                        {
                            td = Regex.Split(rowtext, "<th.*?>(.*?)</th>", RegexOptions.IgnoreCase);
                            for (j = 0; j <= td.Length - 1; j++)
                                excelXml.AppendLine("         <ss:Column ss:Width=\"100\"/>");
                        }
                        else
                            td = Regex.Split(rowtext, "<td.*?>(.*?)</td>", RegexOptions.IgnoreCase);
                        // td = Regex.Split(rowtext, "<th>(.*?)</th>", RegexOptions.IgnoreCase)
                        excelXml.AppendLine("         <ss:Row>");
                        for (j = 0; j <= td.Length - 1; j++)
                        {
                            var coltext = td[j];
                            if (j % 2 != 0)
                            {
                                if (i != 3)
                                    excelXml.AppendLine($@"            <ss:Cell ss:StyleID=""BorderedCell""><ss:Data ss:Type=""String"">{td[j]}</ss:Data></ss:Cell>");

                                else
                                    excelXml.AppendLine($@"            <ss:Cell ss:StyleID=""BoldText""><ss:Data ss:Type=""String"">" + td[j] + "</ss:Data></ss:Cell>");
                            }
                        }
                        excelXml.AppendLine("         </ss:Row>");
                    }
                }
                // Next
                // Excel XML footer
                excelXml.AppendLine("      </ss:Table>");
                excelXml.AppendLine("   </ss:Worksheet>");
                excelXml.AppendLine("</ss:Workbook>");

                return excelXml.ToString();
            }

            public JsonResult sendMail()
            {
                string mailto = Request.Form["Mail_Address"];
                string subject = Request.Form ["mail_subject"];
                string mailbody = Request.Form["mail_text"];

                string report_tomail = Request.Form["report_tomail"];
                report_tomail = report_tomail.Replace("&lt;", "<");
                string chart_tomail = Request.Form["chart_tomail"];
                string report_name = Request.Form["report_name"];

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.Subject = subject;
                mail.From = new System.Net.Mail.MailAddress("shailesh@standus.in");

                string[] mailtos = mailto.Split(";");

                for (var i = 0; i <= mailtos.Length - 1; i++)
                    mail.To.Add(mailtos[i]);

                // Dim url As String = "help.standus.In/TaskUser/Login"

                string body;
                mail.Body = mailbody;
                mail.IsBodyHtml = true;

                string outputpath = GeneratePDF(report_tomail, chart_tomail, report_name);

                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(outputpath);

                mail.Attachments.Add(attachment);
                                
                bool success = SendMail.Send(mail);

                if (success)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });
            }

            public ActionResult ReadExcel()
            {
                //Readexcel f = new Readexcel();
                // f.readdata()
                return View();
            }

            public ActionResult ChangeDefaultValue(int fieldid, string default_value, int change_all, string field_name)
            {
                if (change_all == 0)
                {
                    //ReportFilterContext filters = new ReportFilterContext("ERP");
                    var filter = from fl in _context.ReportFilters 
                                 where fl.Id == fieldid
                                 select fl;
                    ReportFilters flt;
                    flt = filter.First();

                    flt.DefaultValue = default_value;

                    _context.ReportFilters.Entry(flt).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    //ReportFilterContext filters1 = new ReportFilterContext("ERP");
                    var all_filters = from fl in _context.ReportFilters
                                      where fl.Name == field_name
                                      select fl;

                    foreach (ReportFilters fltr in all_filters)
                    {
                        var flt2 = fltr;
                        flt2.DefaultValue = default_value;

                        _context.ReportFilters.Entry(flt2).State = EntityState.Modified;
                    }
                    _context.SaveChanges();
                }
                //ReportFilterContext filters2 = new ReportFilterContext("ERP");
                var filter1 = from fl in _context.ReportFilters 
                              where fl.Id == fieldid
                              select fl;
                ReportFilters flt1;
                flt1 = filter1.First();

                RouteValueDictionary val = new RouteValueDictionary();
                val.Add("reportid", flt1.ReportID);

                return RedirectToAction("ChangeCustomDefaultValue", val);
            }

            public ActionResult ChangeCustomDefaultValue(int reportid)
            {
                //ReportFilterContext filters = new ReportFilterContext("ERP");

                List<ReportFilters> allFilters = _context.ReportFilters.ToList();

                List<ReportFilters> filter = allFilters.Where(fl => fl.ReportID  == reportid && fl.Type == "Custom").ToList();

                return View(filter);
            }

            public ActionResult multiSelect(ReportFilters item) {
                string sql;
                sql = "select " + item.ValueField + "," + item.DisplayField + " from " +
                        item.Table + " order by " + item.DisplayField + " asc";

                var cmd = new SqlCommand();
                var con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
                SqlDataReader dr;
                var dt = new System.Data.DataTable();

                con.Open();
                cmd.CommandText = sql;
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                var listvalues = dt.AsEnumerable()
                                .Select(row => new {ID = row[0],value = row[1]}).ToList();
                return Json(listvalues);
            }

            public IActionResult Preview(int id, string reportName, IFormCollection cntr)
            {
                var cmd = new SqlCommand();
                var con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
                ViewData["conn"] = con;
                // Equivalent to: Dim sql As String
                string sql;

                // Contexts (assumes constructor accepting "ERP" string or configured via DI)
                var reports = _context.Reports.ToList();
                var filters = _context.ReportFilters.ToList();
                var columns = _context.ReportColumns.ToList();
                var dcharts = _context.ReportCharts.ToList();

                var subquery = _context.ReportsSubquery.ToList();

                ViewData["reports"] = reports;
                ViewData["filters"] = filters;
                ViewData["columns"] = columns;
                ViewData["dcharts"] = dcharts;
                ViewData["subquery"] = subquery;

                int reportid = id;
                    
                // ViewData/TempData assignment
                ViewData["ID"] = reportid;
                TempData["ReportID"] = reportid;

                // Sample: this assumes you have some property "ReportName" in your report
                var report = reports.FirstOrDefault(rep => rep.ReportID == reportid);
                TempData["ReportName"] = report?.ReportName;

                // LINQ Queries
                var filts =filters.Where(filt => filt.ReportID == reportid).ToList();
                var cols = columns
                                  .Where(col => col.ReportID == reportid && col.HideColumn == false)
                                  .OrderBy(col => col.ColumnNo)
                                  .ToList();
                var dchart = dcharts.Where(cht => cht.ReportID == reportid).ToList();

                // Connection string (from IConfiguration, not ConfigurationManager)
                string cs = _context.Database.GetDbConnection().ConnectionString;

                // Assuming ViewBag.filters was set somewhere else
                ViewData["ID"] = id;
                ViewData["ReportName"] = reportName;
                ViewData["IsPreview"] = "true";

                var filt_values = new List<FilterValues>();

                foreach (var key in cntr.Keys)
                {
                    // Assuming rFilters is accessible (e.g., injected or declared)
                    var flt = filters
                        .Where(f => f.ReportID == id && f.Name == key)
                        .ToList();

                    if (flt.Any())
                    {
                        filt_values.Add(new FilterValues
                        {
                            Name = key,
                            Value = cntr[key],
                            DefaultValue = flt.First().DefaultValue,
                            type = flt.First().Type
                        });
                    }
                }

                ViewData["filt_values"] = filters;

                return View();
            }
        }
    }


