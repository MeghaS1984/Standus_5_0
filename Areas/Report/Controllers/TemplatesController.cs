using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.Report.Models;
using Standus_5_0.Data;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace Standus_5_0.Areas.Report.Controllers
{
    [Area("Report")]
public class TemplatesController : Controller
{
    private readonly ApplicationDbContext _context;

    public TemplatesController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET: AttendanceHead
    public ActionResult Index()
    {
        var ahs = from ah in _context.Templates
                  select ah;
        return View(ahs);
    }

    // Function Index(id As Integer?) As ActionResult
    // Dim ahs = From ah In _context.Template Select ah
    // Return View(ahs)
    // End Function
    public ActionResult LoadData(int? id)
    {
        ViewData["id"] = id;
        var ahs = from ah in _context.Templates
                  where ah.ID == id
                  select ah;
        return PartialView(ahs);
    }

    // Function LoadData(id As Integer) As ActionResult
    // Dim ahs = From ah In _context.Templates Where ah.ID = id Select ah
    // Return View(ahs)
    // End Function

    // GET: AttendanceHead/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
            return BadRequest("Invalid request");
        Template grd = _context.Templates.Find(id);
        if (grd == null)
            return NotFound("The item you are looking for was not found.");
        return View(grd);
    }

    // GET: AttendanceHead/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: AttendanceHead/Create
    [HttpPost()]
    public ActionResult Create(Template Templates)
    {
        if (ModelState.IsValid)
        {
            _context.Templates.Add(Templates);
            _context.SaveChanges();
            return RedirectToAction("Create");
        }
        return RedirectToAction("index");
    }

    // GET: AttendanceHead/Edit/5
    public ActionResult Edit(int id)
    {
        var ahs = _context.Templates.Find(id);
        return View(ahs);
    }

    // POST: AttendanceHead/Edit/5
    [HttpPost()]
    public ActionResult Edit(int id, Template Templates)
    {
        try
        {
            // TODO: Add update logic here
            if (ModelState.IsValid)
            {
                _context.Templates.Attach(Templates);
                _context.Entry(Templates).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("index");
            }
        }
        catch
        {
        }
        return RedirectToAction("index");
    }

    // GET: AttendanceHead/Delete/5
    public ActionResult Delete(int? id)
    {
        Template tmpl = _context.Templates.Find(id);
        return View(tmpl);
    }

    // POST: AttendanceHead/Delete/5
    [HttpPost()]
    public ActionResult Delete(int id, FormCollection collection)
    {
        try
        {
            Template tmpl = _context.Templates.Find(id);
            _context.Templates.Remove(tmpl);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        catch
        {
            return View();
        }
    }

    public string CreateReportFromTemplate(string screenName, string filterString)
    {
        string template = "";
        SqlConnection con = (SqlConnection)_context.Database.GetDbConnection();
        if (con.State == ConnectionState.Open)
            con.Close();
        con.Open();
        SqlCommand cmd = con.CreateCommand();

        string query = "select * from Template where screen='" + screenName + "'";
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = query;

        // Dim rd As SqlDataReader = cmd.ExecuteReader()
        DataTable dt = new DataTable("Template");
        // dt.Load(rd)
        dt.TableName = "Template";
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.Fill(dt);
        string tb = "";
        if (dt.Rows.Count > 0)
        {
            tb = "<table border=1 width=100%>";
            foreach (DataRow row in dt.Rows)
            {
                RouteValueDictionary rtVal = new RouteValueDictionary();
                rtVal.Add("temlateid", row["templateID"]);
                rtVal.Add("filterValue", 0);
                //UrlHelper ur = new UrlHelper();
                tb = tb + "<tr><td>" + row["Name"] + "</td><td><a href=\"../../../Template/GenerateTemplate?templateID=" + row["templateID"] + "&filterValue=$" + filterString + "\" target=_blank>Show</a></td></tr>";
                // tb = tb & "<tr><td>" & row.Item("Name") & "</td><td>" & ur.Action("GenerateTemplate") & " </td></tr>"
                tb = tb + "</table>";
            }
        }


        return tb;
    }

    public ActionResult filter(string templateID, string filterValue, string screenname)
    {
        var tem = from t in _context.Templates
                  where t.ID == Convert.ToInt32(templateID)
                  select t;

        return View(tem.ToList().First());
    }

    public ContentResult GenerateTemplate(string templateID, string filterValue, string screenname)
    {
        string template = "";
        int maxrows = 0;
        string HideDetailHeader = "";
        string[] values;
        string Pagination = "false";
        string report = "";
        string script = "";
        
        // Dim dt As DataTable

        string designer;
        string DetailHeight = "";

        SqlCommand cmd = new SqlCommand();
        SqlConnection sqlConnection = (SqlConnection)_context.Database.GetDbConnection();
        sqlConnection.Open();
        cmd.Connection = sqlConnection;
        var ssql = "select * from Template where templateID=" + templateID;
        cmd.CommandText = ssql;
        // Dim query As String = "select * from Template where screen='" & screenName & "'"
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(dr);
        string filterstring = dt.Rows[0]["filterstring"].ToString();

        var filters = filterstring.Split(";");
        var filterValues = filterValue.Split("$");

        string sql = dt.Rows[0]["Query"].ToString();

        if (filters.Length > 1)
        {
            for (var i = 0; i <= filters.Length - 2; i++)
                sql = sql.Replace("@" + filters[i] + "@", filterValues[i + 1]);
        }

        designer = dt.Rows[0]["Settings"].ToString();
        if (designer != "")
        {
            values = designer.Split(";");
            if (values[0].Contains("Row"))
                maxrows = Convert.ToInt16(values[0].Split(":").ElementAt(1));
            if (values[1].Contains("HideDetailHeader"))
                HideDetailHeader = values[1].Split(":").ElementAt(1);
            if (values[2].Contains("Pagenation"))
                Pagination = values[2].Split(":").ElementAt(1);
            if (values[3].Contains("DetailHeight"))
                DetailHeight = values[3].Split(":").ElementAt(1);
        }

        // Dim filterStrings As String = dt.Rows(0).Item("FilterString")

        if (Pagination != "True")
        {
            designer = dt.Rows[0]["Header"].ToString();

            sql = Regex.Replace(sql, filterstring, filterValue);
            // query = Regex.Replace(query, "@To@", toDate)
            cmd.CommandText = sql;
            SqlDataReader dataReader = cmd.ExecuteReader();
            DataTable dtdata = new DataTable();
            dtdata.Load(dataReader);
            report = "";

            script = designer;

            if (dtdata.Rows.Count > 0)
            {
                foreach (DataColumn col in dtdata.Columns)
                    script = Regex.Replace(script, "@" + col.ColumnName.ToString() + "@", dtdata.Rows[0][col].ToString());
                
            }
                report = report + script;

                if (dt.Rows[0]["Details"] != DBNull.Value)
                designer = dt.Rows[0]["Details"].ToString();

            string script1 = "";

            if (!HideDetailHeader.Contains("True"))
            {
                //script1 = "<Table width='100%' border='1px' cellpadding='0px' cellspacing='0px' style='height:" + DetailHeight +
                  //  ";min-height:" + DetailHeight + ";'> ";
                // Dim style As String = ""
                // script1= script1 & 
                string title = "<tr style='max-height:15px;'>";
                foreach (DataColumn col in dtdata.Columns)
                {
                    if (designer.Contains(col.ColumnName))
                        title = title + "<td>" + col.ColumnName + "</td>";
                }
                title = title + "</tr>";

                script1 = script1 + title;
            }

            int totalRows = 0;

            // ''' code changed on 11/12/2021 ''''''''''
            foreach (DataRow row in dtdata.Rows)
            {
                string d = designer;
                foreach (DataColumn col in dtdata.Columns)
                    d = Regex.Replace(d, "@" + col.ColumnName.ToString() + "@", row[col].ToString());
                script1 = script1 + d;
                totalRows += 1;
            }
            // '''''''''''''''''''''''''''''''''''''''''
            /// 
            if (totalRows < maxrows)
            {
                int tRows = maxrows - totalRows;
                for (int i = 0; i <= tRows - 1; i++)
                {
                    string d = designer;
                    foreach (DataColumn col in dtdata.Columns)
                        d = Regex.Replace(d, "@" + col.ColumnName.ToString() + "@", "");
                    script1 = script1 + d;
                }
            }

            //script1 = script1 + "</Table>";
            report = report + script1;

            if (dt.Rows[0]["Footer"] != DBNull.Value)
            {
                designer = dt.Rows[0]["Footer"].ToString();
                script = designer;
            }

            if (dtdata.Rows.Count > 0)
            {
                    foreach (DataColumn col in dtdata.Columns)
                    {
                        script = Regex.Replace(script, "@" + col.ColumnName.ToString() + "@", dtdata.Rows[0][col].ToString());
                        
                    }
                    
                }
                report = report + script;
            }
        else
        {
            string query = sql;
            string[] queries = Regex.Split(Regex.Replace(query, "<@PageEnd@>", ""), "<@PageStart@>", RegexOptions.IgnoreCase);

            // Dim sql As String = ""

            sql = Regex.Replace(queries[1], filterstring, filterValue);
            cmd.CommandText = sql;
            dr = cmd.ExecuteReader();
            dt.Load(dr);

            int tPages = (int)(dt.Rows.Count / (double)maxrows);
            tPages = (int)Math.Ceiling((double)tPages);

            designer = dt.Rows[0]["Header"].ToString();

            string[] pages;
            string[] currentPage;
            pages = Regex.Split(designer, "<@PageStart@>", RegexOptions.IgnoreCase);

            for (int i = 1; i <= pages.Length  - 1; i++)
            {
                string pagh = pages[i];
                sql = queries[i];
                sql = Regex.Replace(sql, filterstring, filterValue);

                cmd.CommandText = sql;
                dr = cmd.ExecuteReader();
                dt.Load (dr);

                if (dt.Rows.Count > 0)
                {
                    pagh = Regex.Replace(pagh, "<@PageEnd@>", "");
                    foreach (DataColumn col in dt.Columns)
                        pagh = Regex.Replace(pagh, "@" + col.ColumnName.ToString() + "@", dt.Rows[0][col].ToString());
                    report = report + pagh;
                }

                designer = dt.Rows[0]["Details"].ToString();

                string script1 = "";

                if (!HideDetailHeader.Contains("True"))
                {
                    script1 = "<Table width='100%' border='1px' cellpadding=0 cellspacing=0 style='height:" + DetailHeight + "'>";
                    string title = "<tr style='height:15px;'>";
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (designer.Contains(col.ColumnName))
                            title = title + "<td>" + col.ColumnName + "</td>";
                    }
                    title = title + "</tr>";

                    script1 = script1 + title;
                }

                int totalRows = 0;

                string pagerowdata = "";
                if (designer.Contains("<@PageStart@>"))
                {
                    string[] paged = Regex.Split(designer, "<@PageStart@>", RegexOptions.IgnoreCase);
                    // For Each Pag As String In pages
                    string pag = paged[i];
                    if (pag != "")
                    {
                        string[] dd = Regex.Split(pag, "<table id=\"page-start\"></table>", RegexOptions.IgnoreCase);
                        // Dim row As String
                        foreach (string d in dd)
                        {
                            string l = d.Length.ToString();

                            if (d.Length > 2)
                            {
                                string rowdata = Regex.Replace(d, "<@PageEnd@>", "");
                                rowdata = Regex.Replace(rowdata, "<table id=\"page-end\"></table>", "");

                                string newrow = "";
                                foreach (DataRow row in dt.Rows)
                                {
                                    newrow = rowdata;
                                    foreach (DataColumn col in dt.Columns)
                                        newrow = Regex.Replace(newrow, "@" + col.ColumnName.ToString() + "@", row[col].ToString());
                                    pagerowdata = pagerowdata + newrow;
                                    totalRows += 1;
                                }
                                // script1 = script1 & "<table id=""page-start""></table>" & pagerowdata & "<table id=""page-end""></table>"
                                // script1 = script1 & pagerowdata
                                script1 = script1 + pagerowdata;
                                pagerowdata = "";

                                // '''''''''''' decommented on 22.06.2022
                                if (totalRows < maxrows)
                                {
                                    int tRows = maxrows - totalRows;
                                    for (int j = 0; j <= tRows - 1; j++)
                                    {
                                        newrow = rowdata;
                                        foreach (DataColumn col in dt.Columns)
                                            newrow = Regex.Replace(newrow, "@" + col.ColumnName.ToString() + "@", "");
                                        script1 = script1 + newrow;
                                    }
                                }
                            }
                        }
                    }
                }

                script1 = script1 + "</Table>";
                report = report + script1;

                designer = dt.Rows[0]["Footer"].ToString();

                string[] pagef = Regex.Split(designer, "<@PageStart@>", RegexOptions.IgnoreCase);

                string pagf = pagef[i];
                // script = designer
                string scriptFooter = "";
                if (dt.Rows.Count > 0)
                {
                    pagf = Regex.Replace(pagf, "<@PageEnd@>", "");
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.Contains("Freight"))
                            //Int16 z = 1;
                        pagf = Regex.Replace(pagf, "@" + col.ColumnName.ToString() + "@", dt.Rows[0][col].ToString());
                    }
                    script = pagf;
                }
                report = report + script;
            }
        }

        template = report;

        template = Regex.Replace(template, "'", @"\'");
        template = Regex.Replace(template, ",", " ");
        template = template.Replace("\r", "").Replace("\n", "");

        // script = "reportFormat2('" & content & "');"

        // ScriptManager.RegisterStartupScript(Page, Me.GetType, "Script", script, True)

        ViewBag.report = template;

        return Content(template);
    }

    public ActionResult CopyTemplate(int TemplateID)
    {
        var copyFrom = from tem in _context.Templates
                       where tem.ID == TemplateID
                       select tem;

        Template copyto = new Template();

        copyto = copyFrom.ToList().First();
        copyto.ID = 0;
        // copyto.ID = 0

        _context.Templates.Add(copyto);

        _context.SaveChanges();

        return RedirectToAction("index");
    }
}
}
