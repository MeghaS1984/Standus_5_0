using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.Report.Models;
using Standus_5_0.Data;
using Microsoft.Data.SqlClient;


namespace Standus_5_0.Areas.Report.Controllers
{
    [Area("Report")]
    public class ReportsController : Controller
    {
        //private ReportContext rep = new ReportContext("ERP");
        //private GroupContext grp = new GroupContext("ERP");
        //private ReportFilterContext Filters = new ReportFilterContext("ERP");
        //private ReportColumnContext Columns = new ReportColumnContext("ERP");
        //private ReportChartContext Charts = new ReportChartContext("ERP");

        //private string CS = ConfigurationManager.ConnectionStrings("datastore").ConnectionString;

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReportsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public ActionResult NewReport()
        {
            return View();
        }
        public ActionResult Index()
        {
            var rep = _context.Reports
                .Include(r => r.Columns)
                .Include(r => r.ExcelColumns)
                .Include(r => r.CustomFields)
                .Include(r => r.group);

            //var group = new List<SelectListItem> {
            //    new SelectListItem { Text = "Select Group" , Value = "0" }
            //};

            //group.AddRange(_context.Groups.Select(d => new SelectListItem
            //{
            //    Text = d.Groupname,
            //    Value = d.GroupID.ToString()
            //}).OrderBy(d => d.Text));

            ViewData["Group"] = _context.Groups.OrderBy(f => f.Groupname);
            return View(rep.ToList());
        }

        //[HttpGet]
        public ActionResult Designer(string ReportType,int Id)
        {
            //string ReportType,int Id
            ViewData["IsDesigner"] = true;
            ViewData["ReportType"] = ReportType;

            ViewData["Group"] = _context.Groups.OrderBy(o => o.Groupname).ToList();  

            if (Id != 0)
            {
                var viewModel = new ReportComponent();

                viewModel.Report = _context.Reports.Where(report => report.ReportID == Id).First();

                ViewData["ReportName"] = viewModel.Report.ReportName;

                viewModel.Filter = _context.ReportFilters.Where(filter => filter.ReportID == Id).ToList();

                viewModel.Column = _context.ReportColumns.Where(column => column.ReportID == Id).ToList();

                viewModel.Chart = _context.ReportCharts.Where(chart => chart.ReportID == Id).ToList();

                return View(viewModel);
            }
            return View(new ReportComponent());
        }

        // GET: Report
        [HttpPost()]
        public ActionResult Setting(int id, Reports report)
        {
            // If ModelState.IsValid Then
            RouteValueDictionary val = new RouteValueDictionary();
            val.Add("id", id);
            if (id == 0)
            {
                TempData["Error"] = "Invalid Report";
                return RedirectToAction("Designer", val);
            }
            var sql = "update reports set AutoMailto='" + report.AutoMailTo + "',PDFPageSize='" + report.PDFPageSize + "' where reportID=" + id;
            _context.Database.ExecuteSqlRaw(sql);

            return RedirectToAction("Designer", val);
        }
        [HttpGet()]
        //public ActionResult Designer(int id)
        //{
        //    ViewData["IsDesigner"] = "true";

        //    if (id != null)
        //    {
        //        ReportComponent viewModel = new ReportComponent();

        //        viewModel.Report = (from report in _context.Reports
        //                                     where report.ReportID == id
        //                                     select report).First();

        //        ViewData["ReportName"] = viewModel.Report.ReportName;

        //        viewModel.Filter = (from filter in _context.ReportFilters
        //                            where filter.ReportID == id
        //                            select filter).ToList();

        //        viewModel.Column = (from column in _context.ReportColumns
        //                            where column.ReportID == id
        //                            select column).ToList();

        //        viewModel.Chart = (from chart in _context.ReportCharts
        //                           where chart.ReportID == id
        //                           select chart).ToList();

        //        return View(viewModel);
        //    }
        //    return View(new ReportComponent());
        //}


        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost()]
        public ActionResult Create(Reports report, ReportColumns cols)
        {
            IDbContextTransaction trans;
            int id = 0;
            trans = _context.Database.BeginTransaction();
            ModelState.Remove("group");
            ModelState.Remove("Report");
            ModelState.Remove("Columns"); 
            ModelState.Remove("GroupBy");
            ModelState.Remove("FilePath");
            ModelState.Remove("Ref_Table");
            ModelState.Remove("AutoMailTo");
            ModelState.Remove("ColumnName");
            ModelState.Remove("PDFPageSize");
            ModelState.Remove("SummaryType");
            ModelState.Remove("CustomFields");
            ModelState.Remove("ExcelColumns");
            ModelState.Remove("SummeryType");
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {
                    

                    var isExists = from rp in _context.Reports
                                   where rp.ReportID == report.ReportID
                                   select rp;

                    if (isExists.Count() > 0)
                    {

                        // For Each chrt As ReportCharts In report.dCharts
                        // chrt.ReportID = report.ReportID
                        // Next

                        Update_report(report);

                        trans.Commit();

                        RouteValueDictionary valr = new RouteValueDictionary();
                        valr.Add("id", report.ReportID);
                        return RedirectToAction("Designer", valr);                                                                
                    }

                    report.AutoMailTo = "";
                    report.PDFPageSize = "";
                    report.FilePath = "";
                    report.Ref_Table = "";
                    report.Date_Created = DateTime.Now;

                    _context.Entry(report).State = EntityState.Added;
                    _context.SaveChanges();

                    id = report.ReportID;

                    string sql;

                    // Dim filtername As String()
                    // filtername = report.SqlQuery.Split("@")
                    // If filtername.Count > 0 Then
                    // For i As Integer = 0 To filtername.Count - 1
                    // If ((i + 1) Mod 2) = 0 Then
                    // Dim name As String = filtername(i).Replace("@", "")
                    // Dim filter As New ReportFilters
                    // filter.ReportID = id
                    // filter.Name = name
                    // Filters.Filters.Add(filter)
                    // Filters.SaveChanges()
                    // sql = sql.Replace("@" & name & "@", "-1")
                    // End If
                    // Next
                    // End If

                    // Dim dt As New DataTable
                    // Dim conn As New SqlConnection(CS)
                    // Dim dr As IDataReader
                    // Dim cmd As New SqlCommand

                    // sql = report.SqlQuery

                    // cmd.Connection = conn
                    // cmd.CommandType = CommandType.Text
                    // cmd.CommandText = sql
                    // conn.Open()
                    // dr = cmd.ExecuteReader

                    // dt.Load(dr)

                    // 'Dim dtc() As DataColumn
                    // 'dt.Columns.CopyTo(dtc, 0)
                    // Dim batchSize = 1

                    // Dim parameters As SqlParameter()

                    // Using context As New ReportColumnContext("ERP")
                    // sql = "INSERT INTO ReportColumns (ReportID, ColumnName, Width) VALUES (@ReportID, @ColumnName, @Width)"

                    // For Each col As DataColumn In dt.Columns
                    // parameters = {
                    // New SqlParameter("@ReportID", id),
                    // New SqlParameter("@ColumnName", col.ColumnName),
                    // New SqlParameter("@Width", 200)
                    // }

                    // context.Database.ExecuteSqlCommand(sql, parameters)
                    // Next
                    // End Using

                    // sql = "INSERT INTO ReportCharts (ReportID) VALUES (@ReportID)"

                    // 'For Each chart As ReportChart In report.Charts
                    // parameters = {
                    // New SqlParameter("@ReportID", id)
                    // }

                    // Charts.Database.ExecuteSqlCommand(sql, parameters)
                    // 'Next

                    trans.Commit();
                }

                RouteValueDictionary val = new RouteValueDictionary();
                val.Add("id", id);
                return RedirectToAction("Designer", val);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ViewData["Error"] = ex.Message;
                return RedirectToAction("Designer");
            }
        }

        public ActionResult Update_report(Reports report)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    // rep.Entry(report).State = EntityState.Modified
                    // rep.SaveChanges()

                    string sql;

                    sql = "Update Reports set ReportName=@ReportName,SQlQuery=@SqlQuery,GroupID=@GroupID,Date_updated=@dateupdated where ReportID=@ReportID";
                                        
                    SqlParameter[] parameters;
                    
                    //using (ReportColumnContext context = new ReportColumnContext("ERP"))
                    {
                        parameters = new[] {
                            new SqlParameter("@ReportId", report.ReportID),
                            new SqlParameter("@ReportNAme", report.ReportName),
                            new SqlParameter("@SqlQuery", report.SqlQuery),
                            new SqlParameter("@GroupId", report.GroupID),
                            new SqlParameter("@ReportType", report.ReportType),
                            new SqlParameter("@dateupdated", DateTime.Now)
                        };
                        _context.Database.ExecuteSqlRaw(sql, parameters);
                    }
                }

                RouteValueDictionary val = new RouteValueDictionary();
                val.Add("id", report.ReportID);
                return RedirectToAction("Designer", val);
            }
            catch (Exception ex)
            {
                RouteValueDictionary val = new RouteValueDictionary();
                val.Add("id", report.ReportID);
                return RedirectToAction("Designer", val);
            }
        }

        [HttpPost()]
        public ActionResult NewGroup(Standus_5_0.Areas.Report.Models.Groups Group)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    _context.Groups.Add(Group);
                    _context.SaveChanges();
                }
                return RedirectToAction("Designer");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(ReportColumns col)
        {
            return View();
        }

        // POST: Report/Edit/5
        [HttpPost()]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // Function columns(ByVal id As Integer) As ActionResult
        // Return View()
        // End Function

        // POST: Report/Delete/5
        [HttpPost()]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete_column(int id)
        {
            try
            {
                // TODO: Add delete logic here
                ReportColumns col = _context.ReportColumns.Find(id);
                if (col != null)
                {
                    int reportid = col.ReportID;
                    _context.ReportColumns.Remove(col);
                    _context.SaveChanges();
                }
                RouteValueDictionary v = new RouteValueDictionary();
                v.Add("id", col.ReportID);
                return RedirectToAction("Designer", v);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Add_column(ReportColumns col)
        {
            RouteValueDictionary v = new RouteValueDictionary();
            v.Add("id", col.ReportID);
            try
            {
                // TODO: Add delete logic here
                if (col.ReportID > 0)
                {
                    if (col.GroupBy == null)
                        col.GroupBy = "";
                    _context.ReportColumns.Add(col);
                    _context.SaveChanges();
                    return RedirectToAction("Designer", v);
                }
                else
                {
                    TempData["Error"] = "Invalid Report";
                    return RedirectToAction("Designer", v);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Designer", col.ReportID);
            }
        }

        public ActionResult Add_filters(ReportFilters filts)
        {
            RouteValueDictionary v = new RouteValueDictionary();
            v.Add("id", filts.ReportID);
            try
            {
                // TODO: Add delete logic here
                if (filts.ReportID > 0)
                {
                    // filts.Report.ReportType = ""
                    _context.ReportFilters.Add(filts);
                    _context.SaveChanges();
                    return RedirectToAction("Designer", v);
                }
                else
                {
                    TempData["Error"] = "Invalid Report";
                    return RedirectToAction("Designer", v);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException.ToString();
                return RedirectToAction("Designer", v);
            }
        }

        public ActionResult Delete_filter(int id)
        {
            RouteValueDictionary v = new RouteValueDictionary();

            try
            {
                // TODO: Add delete logic here
                ReportFilters filt = _context.ReportFilters.Find(id);
                int reportid = filt.ReportID;
                _context.ReportFilters.Remove(filt);
                _context.SaveChanges();
                v.Add("id", reportid);
                return RedirectToAction("Designer", v);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Add_chart(ReportCharts cht)
        {
            RouteValueDictionary v = new RouteValueDictionary();
            v.Add("id", cht.ReportID);
            try
            {
                // TODO: Add delete logic here
                if (cht.ReportID > 0)
                {
                    // filts.Report.ReportType = ""
                    _context.ReportCharts.Add(cht);
                    _context.SaveChanges();
                    return RedirectToAction("designer", v);
                }
                else
                {
                    TempData["Error"] = "Invalid Report";
                    return RedirectToAction("designer", v);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException.ToString();
                return RedirectToAction("designer", v);
            }
        }

        public ActionResult Delete_chart(int id)
        {
            RouteValueDictionary v = new RouteValueDictionary();
            v.Add("id", id);
            try
            {
                // TODO: Add delete logic here
                ReportCharts cht = _context.ReportCharts.Find(id);
                int reportid = cht.ReportID;
                _context.ReportCharts.Remove(cht);
                _context.SaveChanges();
                return RedirectToAction("Designer", v);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException.ToString();
                return RedirectToAction("Designer", v);
            }
        }

        public ActionResult CreateExcel(string type)
        {
            ViewData["type"] = type;
            ViewData["Group"] = _context.Groups.ToList();  
            return View();
        }
        [HttpPost()]
        public ActionResult CreateExcel(Reports rpt, ExcelColumns exl,IFormFile FilePath)
        {
            try
            {
                string rootPath = _env.ContentRootPath;      // Points to the root of the project (where appsettings.json is)
                string webRootPath = _env.WebRootPath;       // Points to wwwroot

                string filename = Path.Combine(webRootPath, "myfile.txt");
                //string filename = Server.MapPath("~");
                rpt.ExcelColumns.Remove(rpt.ExcelColumns.Last());
                rpt.AutoMailTo = "";
                rpt.PDFPageSize = "";
                rpt.Ref_Table = "";
                rpt.FilePath = @"E:\" + FilePath.FileName;
                rpt.ReportType = "Excel Import";
                _context.Reports.Add(rpt);
                _context.SaveChanges();

                int reportid = rpt.ReportID;

                int i = 0;
                var sql = "create table ExcelTable" + reportid + "(";
                foreach (ExcelColumns exls in rpt.ExcelColumns)
                {
                    if (i > 0)
                        sql = sql + ",";
                    sql = sql + "[" + exls.ColumnName + "] varchar(max)";
                    i = i + 1;
                }
                sql = sql + ", [ImportDateTime] varchar(max),ID int not null default(0))";

                //DbContext dbcon = new DbContext("ERP");
                _context.Database.ExecuteSqlRaw(sql);

                rpt.Ref_Table = "ExcelTable" + reportid;
                _context.Reports.Entry(rpt).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "True";
                ViewData["Message"] = ex.InnerException.InnerException;
            }
            return RedirectToAction("CreateExcel",new { type= "Excel Import" }) ;
        }

        public ActionResult CopyReport(Reports copy_report)
        {
            Reports r = new Reports();
            // r.First.ReportID = 0
            r.SqlQuery = copy_report.SqlQuery;
            r.GroupID = copy_report.GroupID;
            r.ReportName = copy_report.ReportName;
            r.ReportType = copy_report.ReportType;
            r.AutoMailTo = "";
            r.PDFPageSize = "";
            r.FilePath = "";
            r.Ref_Table = "";
            r.Date_Created = DateTime.Now;

            _context.Reports.Add(r);
            _context.SaveChanges();

            //DbContext db = new DbContext("ERP");
            string sql;

            int newid = r.ReportID;

            var f = from filter in _context.ReportFilters 
                    where filter.ReportID == copy_report.ReportID
                    select filter;

            if (f == null)
            {
            }
            else
                foreach (ReportFilters flt in f)
                {
                    sql = "insert into ReportFilters values(" + newid + ",'" + flt.Name + "','" + flt.Type + "',";
                    sql = sql + "'" + flt.Table + "','" + flt.ValueField + "','" + flt.DisplayField + "','" + flt.DefaultValue + "','" + flt.ShowAll + "',";
                    sql = sql + "'" + flt.CustomFilter + "','" + flt.LinkField + "')";

                    _context.Database.ExecuteSqlRaw(sql);
                }

            var c = from column in _context.ReportColumns 
                    where column.ReportID == copy_report.ReportID
                    select column;

            if (c == null)
            {
            }
            else
                foreach (ReportColumns clm in c)
                {
                    sql = "insert into ReportColumns values(" + newid + ",'" + clm.ColumnName + "',";
                    sql = sql + "" + clm.Width + ",'" + clm.SummeryType + "','" + clm.LinkTo + "'," + clm.ColumnNo + ",";
                    sql = sql + "'" + clm.GroupBy + "'," + clm.GroupLevel + "," + (clm.HideDuplicates? 1: 0) + "," + (clm.HideColumn? 1: 0) + ")";
                    _context.Database.ExecuteSqlRaw(sql);
                }

            var ch = from chart in _context.ReportCharts 
                     where chart.ReportID == copy_report.ReportID
                     select chart;

            if (ch == null)
            {
            }
            else
                foreach (ReportCharts cht in ch)
                {
                    sql = "insert into ReportCharts values(" + newid + ",'" + cht.ChartType + "','" + cht.XValue + "',";
                    sql = sql + "'" + cht.YOValue + "','" + (cht.YTValue == "None"? 0: cht.YTValue) + "')";
                    _context.Database.ExecuteSqlRaw(sql);
                }


            RouteValueDictionary v = new RouteValueDictionary();
            v.Add("id", newid);

            return RedirectToAction("index", "report");
        }

        public ActionResult delete_report(int reportid)
        {
            //DbContext context = new DbContext("ERP");

            _context.Database.ExecuteSqlRaw("delete from reports where reportid=" + reportid);

            _context.Database.ExecuteSqlRaw("delete from reportcolumns where reportid=" + reportid);

            _context.Database.ExecuteSqlRaw("delete from reportfilters where reportid=" + reportid);

            _context.Database.ExecuteSqlRaw("delete from reportcharts where reportid=" + reportid);

            _context.Database.ExecuteSqlRaw("delete from ExcelColumns where reportid=" + reportid);

            return RedirectToAction("index", "report");
        }

        public ActionResult Hide_Unhide_Report(int id, int hide)
        {

            // Dim rpt = From rp As Reports In rep.Reports where rp.reportid=id
            // rpt.First.Hide = hide
            // rep.Entry(rpt).State = EntityState.Modified
            // rep.SaveChanges()
            // hide = IIf(hide=1,0,1)
            var sql = "update reports set hide=" + hide + " where reportid=" + id;

            SqlConnection con = (SqlConnection)_context.Database.GetDbConnection();
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("index", "report");
        }

        public ActionResult change_name(int id, string changeto)
        {
            var sql = "update reports set reportname='" + changeto + "' where reportid=" + id;

            SqlConnection con = (SqlConnection)_context.Database.GetDbConnection();
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("index", "report");
        }
    }
}
