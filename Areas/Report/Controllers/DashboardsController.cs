using Microsoft.VisualBasic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Standus_5_0.Data;
using Microsoft.AspNetCore.Authorization;

namespace Standus_5_0.Areas.Report.Controllers
{
    [Area("Report")]
    public class DashboardsController : Controller
        {
        private readonly ApplicationDbContext _context;

        public DashboardsController(ApplicationDbContext context)
        {
            _context = context;
        }

            // GET: Dashboards
            public ActionResult Index()
            {
                return View(_context.Dashboards.ToList());
            }

            public ActionResult Preview()
            {
            TempData["cs"] = _context.Database.GetDbConnection().ConnectionString; 
                return View(_context.Dashboards.ToList());
            }
            // GET: Dashboards/Details/5
            public ActionResult Details(int? id)
            {
                if (Information.IsNothing(id))
                    return BadRequest();
                Standus_5_0.Areas.Report.Models.Dashboard dashboard = _context.Dashboards.Find(id);
                if (dashboard == null)
                    return NotFound();
                return View(dashboard);
            }

            // GET: Dashboards/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Dashboards/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost()]
            [ValidateAntiForgeryToken()]
            public ActionResult Create(Standus_5_0.Areas.Report.Models.Dashboard dashboard)
            {
                //if (ModelState.IsValid)
                //{
                    _context.Dashboards.Add(dashboard);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                //}
                return View(dashboard);
            }

            // GET: Dashboards/Edit/5
            public ActionResult Edit(int? id)
            {
                if (Information.IsNothing(id))
                    return BadRequest();
            Standus_5_0.Areas.Report.Models.Dashboard dashboard = _context.Dashboards.Find(id);
                if (dashboard == null)
                    return NotFound();
                return View(dashboard);
            }

            // POST: Dashboards/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost()]
            [ValidateAntiForgeryToken()]
            public ActionResult Edit(Standus_5_0.Areas.Report.Models.Dashboard dashboard)
            {
                
                    _context.Entry(dashboard).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
               
                //return View(dashboard);
            }

            // GET: Dashboards/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                    return BadRequest();

            Standus_5_0.Areas.Report.Models.Dashboard dashboard = _context.Dashboards.Find(id);

                if (dashboard == null)
                    return NotFound();
                return View(dashboard);
            }

            // POST: Dashboards/Delete/5
            [HttpPost()]
            [ActionName("Delete")]
            [ValidateAntiForgeryToken()]
            public ActionResult DeleteConfirmed(int id)
            {
                Standus_5_0.Areas.Report.Models.Dashboard dashboard = _context.Dashboards.Find(id);
                _context.Dashboards.Remove(dashboard);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            protected override void Dispose(bool disposing)
            {
                if ((disposing))
                    _context.Dispose();
                base.Dispose(disposing);
            }

            public ActionResult updateDash(int id, string filter)
            {
            Standus_5_0.Areas.Report.Models.Dashboard dashboard = _context.Dashboards.Find(id);

                string CS = _context.Database.GetConnectionString();

                string sql;
                sql = dashboard.query;

                int i;
                string[] filters = filter.Split("$");

                for (i = 1; i <= filters.Length - 1; i += 2)
                    sql = sql.Replace("@" + filters[i] + "@", filters[i + 1]);

                SqlCommand cmd = new SqlCommand();
                SqlConnection con = new     SqlConnection(CS);
                SqlDataReader dr;
                System.Data.DataTable dt = new System.Data.DataTable();

                con.Open();
                cmd.CommandText = sql;
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
                dt.Load(dr);

                string tabledata = "";
                tabledata = tabledata + "<tr>";
                foreach (System.Data.DataColumn col in dt.Columns)
                    tabledata = tabledata + "<th>" + col.ColumnName + "</th>";
                tabledata = tabledata + "</tr>";
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    tabledata = tabledata + "<tr>";
                    foreach (System.Data.DataColumn col in dt.Columns)
                        tabledata = tabledata + "<td>" + row[col.ColumnName] + "</td>";
                    tabledata = tabledata + "</tr>";
                }

                return Content(tabledata, "text/plain");
            }

            public JsonResult chartdata(int ID, string filter)
            {
                Standus_5_0.Areas.Report.Models.Dashboard dashboard = _context.Dashboards.Find(ID);

                string CS = _context.Database.GetConnectionString();

                string sql;
                sql = dashboard.query;

                int i;

                string[] filters;

                if (filter != null)
                {
                    filters = filter.Split("$");

                    for (i = 1; i <= filters.Length - 1; i += 2)
                        sql = sql.Replace("@" + filters[i] + "@", filters[i + 1]);
                }
                System.Data.DataTable dtt = new System.Data.DataTable();
                SqlConnection conn = new SqlConnection(CS);
                System.Data.IDataReader drr;
                SqlCommand cmd = new SqlCommand();

                var xvalue = dashboard.xvalue;
                var yovalue = dashboard.yovalue ;
                var ytvalue = dashboard.ytvalue;

                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                conn.Open();
                drr = cmd.ExecuteReader();

                dtt.Load(drr);

                xvalue = xvalue.Replace("[", "");
                xvalue = xvalue.Replace("]", "");
                if (ytvalue != null)
                {
                    ytvalue = ytvalue.Replace("[", "");
                    ytvalue = ytvalue.Replace("]", "");
                }

                yovalue = yovalue.Replace("[", "");
                yovalue = yovalue.Replace("]", "");

                List<object> iData = new List<object>();
                DataTable dt = new DataTable();
                dt.Columns.Add("'" + xvalue + "'", System.Type.GetType("System.String"));
                dt.Columns.Add("'" + yovalue + "'", System.Type.GetType("System.Int32"));
                dt.Columns.Add("'" + ytvalue + "'", System.Type.GetType("System.Int32"));
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
                    var x = (from drr1 in dt.Rows.Cast<DataRow>()
                             select drr1[dc.ColumnName]).ToList();
                    iData.Add(x);
                }

                return Json(iData);
            }
        }  

}
