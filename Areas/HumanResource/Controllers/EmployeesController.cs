using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employee
                .Include(e => e.Position)
                .Include(e => e.Department);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: HumanResource/Employees/Create
        public IActionResult Create()
        {
            PopulateSelect();
            return View();
        }

        public void PopulateSelect(int PostionID = 0,int DepartmentID = 0) {

            var pos = new List<SelectListItem>
            {
                new SelectListItem{Text = "Select Position" , Value = "0" }
            };

            pos.AddRange(_context.Position.Select(p => new SelectListItem
            {
                Text = p.PositionName,
                Value = p.PositionID.ToString()
            }).OrderBy(d => d.Text) );


            ViewData["PositionID"] = new SelectList(pos, "Value", "Text",PostionID);

            var dept = new List<SelectListItem> {
                new SelectListItem { Text = "Select Department" , Value = "0" }
            };

            dept.AddRange(_context.Department.Select(d => new SelectListItem
            {
                Text = d.DepartmentName,
                Value = d.DepartmentID.ToString()
            }).OrderBy(d => d.Text));

            //var dept = _context.Department.OrderBy(d => d.DepartmentName);              
            ViewData["DepartmentID"] = new SelectList(dept, "Value", "Text", DepartmentID);
        }

        // POST: HumanResource/Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Name,Email,Phone,Address,DateOfBirth,HireDate,Salary,DepartmentID,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                //employee.Name = employee.FirstName + " " + employee.LastName;
                _context.Add(employee);
                await _context.SaveChangesAsync();

                if (employee.EmployeeID > 0)
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Employee added");
                }
                else
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Unknown error");
                }
                //return RedirectToAction(nameof(Index));
                return View(employee);
            }
            else {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Unknown error");
            }
            ViewData["PositionID"] = new SelectList(_context.Set<Position>(), "PositionID", "PositionName", employee.PositionID);
            return View(employee);
        }

        // GET: HumanResource/Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = id;
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            PopulateSelect(employee.PositionID, employee.DepartmentID);
            return View(employee);
        }

        // POST: HumanResource/Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Email,Phone,Address,DateOfBirth,HireDate,Salary,DepartmentID,PositionID")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            PopulateSelect(employee.PositionID,employee.DepartmentID );
            return RedirectToAction(nameof(Index),new { id = employee.EmployeeID});
        }
        // GET: HumanResource/Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: HumanResource/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeID == id);
        }
        public async Task<IActionResult> Allowance(int employeeid, int categoryid)
        {

            ViewData["employeeid"] = employeeid;
            ViewData["categoryid"] = categoryid;

            var allowance = from A in _context.Allowance
                         join B in _context.Slab on A.ID equals B.AllowanceID
                         join C in _context.SlabCategory on B.SlabID equals C.SlabID
                         join D in _context.SlabCalculation on B.SlabID equals D.SlabID
                         join E in _context.SlabDetails  on D.DetailsID equals E.ID 
                         where C.CategoryID == (categoryid == null ? 0 : Convert.ToInt32(categoryid))
                         && B.SlabID == E.SlabID
                         orderby A.PayrollSlNO
                            select new AllowanceDetails
                            {
                                ID = A.ID,
                                Name = A.Name,
                                Amount = 0,
                                Fixed = A.Fixed,
                                FromAmount = 0,
                                ToAmount = 0,
                                Employee = 0,
                                Employer = 0,
                                DetailsID = E.ID
                            };


            var slab = from A in _context.SlabAllowance
                         join B in _context.Allowance on A.AllowanceID equals B.ID
                         where A.EmployeeID == (employeeid == null ? 0 : Convert.ToInt32(employeeid))
                         orderby B.PayrollSlNO
                         select new
                         {
                             A.AllowanceID,
                             B.Name,
                             Amount = 0,
                             B.Fixed,
                             A.FromAmount,
                             A.ToAmount,
                             A.Employee,
                             A.Employer,
                             A.Type
                         };

            // Merging the two lists based on DeductionID
            var allowance_set = allowance.ToList().Join(slab,
                                d => d.ID,
                                s => s.AllowanceID,
                                (d, s) =>
                                {
                                    d.Amount = s.Amount;
                                    d.Fixed = s.Fixed;
                                    d.FromAmount = s.FromAmount;
                                    d.ToAmount = s.ToAmount;
                                    d.Employee = s.Employee;
                                    d.Employer = s.Employer;
                                    return d;
                                }).ToList();

            return View(allowance_set);
        }

        public ContentResult SetupAllowance(int employeeid, int categoryid)
        {
            {
                var cmd = new SqlCommand();
                var dtAll = new DataTable();
                var dt = new DataTable();

                //cmd.CommandText = "select EMployeeID from EMployee where CategoryID=" + categoryid;

                // Open the connection from the DbContext
                var connection = _context.Database.GetDbConnection(); // Get the DB connection from the DbContext

                // Ensure the connection is open before executing the query
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();  // Open the connection
                }

                // Set the command properties
                cmd.CommandType = CommandType.Text;  // Command type is SQL text
                cmd.Connection = (SqlConnection)connection;  // Set the connection to the command

                // Example query (replace with actual SQL query)
                cmd.CommandText = "SELECT * FROM Allowance";  // Replace with your actual query

                // Create a SqlDataAdapter to execute the command
                using (var sda = new SqlDataAdapter(cmd))
                {
                    // Fill the DataTable with the query result
                    sda.Fill(dtAll);
                }

                foreach (DataRow ra in dtAll.Rows)
                {
                    var Employee = default(double);
                    var Employer = default(double);

                    int allowanceid = (int)ra["AllowanceID"];

                    cmd.CommandText = "select distinct G.AllowanceID ,G.Employer,OnIncome from Allowance A  " +
                        "Inner join Slab B On A.AllowanceID =B.AllowanceID  " +
                        "inner join SlabDetails C On C.SlabID =B.SlabID " +
                        "inner join Category D on C.CategoryID =D.CategoryID " +
                        "inner Join SlabCalculation E on C.DetailsID = E.DetailsID and B.SlabID =E.SlabID " +
                        "inner join Allowance F On E.AllowanceID = F.AllowanceID " +
                        "inner join SlabAllowance G on F.AllowanceID = G.AllowanceID  " +
                        "where A.AllowanceID = " + allowanceid + " And C.CategoryID = " + categoryid + " " +
                        "And G.EmployeeID = " + employeeid + "";

                    using (var sda = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataTable with the query result
                        sda.Fill(dt);
                    }

                    double Allowance = 0d;

                    string OnIncome = "";

                    foreach (DataRow row in dt.Rows)
                    {

                        if (row["OnIncome"] == "Monthly")
                        {
                            Allowance += (double)row.ItemArray[1];
                        }
                        else if (row["OnIncome"] == "Yearly Income")
                        {
                            double value = Convert.ToDouble(row.ItemArray[1]);  // Convert to double
                            Allowance += value * 12;
                            OnIncome = "Yearly";
                        }
                    }

                    cmd.CommandText = "delete from SlabAllowance where EmployeeID=" + employeeid + "" +
                    "and allowanceID=" + allowanceid;

                    cmd.ExecuteNonQuery();

                    string Sql = "select * from Slab A " +
                    "inner join SlabDetails B on A.SlabID =B.SlabID " +
                    "inner join Allowance C On A.AllowanceID =C.AllowanceID " +
                    "where A.allowanceID = " + allowanceid + " And B.CategoryID = " + categoryid + " ";



                    cmd.CommandText = Sql;
                    using (var sda = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataTable with the query result
                        sda.Fill(dt);
                    }

                    foreach (DataRow r in dt.Rows)
                    {
                        double value = Convert.ToDouble(r["FromAmount"]);
                        double FromAmount = value;
                        value = Convert.ToDouble(r["FromAmount"]);
                        double ToAmount = value;

                        bool apply = false;

                        if (Convert.ToDouble(r["FromAmount"]) == 0 & Convert.ToDouble(r["ToAmount"]) == 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string @type = r["type"].ToString();
                                if (type == "Percent")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0);
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employer"]) / 100), 0);
                                }
                                else if (type == "Amount")
                                {
                                    Employee = Convert.ToDouble(r["employee"]);
                                    Employer = Convert.ToDouble(r["employer"]);
                                }
                                if (OnIncome == "Yearly")
                                {
                                    Employee = Math.Round(Allowance * Convert.ToDouble(r["employee"]) / 100, 0) / 12;
                                    r["employee"] = Employee;
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                }
                            }
                            apply = true;
                        }
                        else if (Allowance >= Convert.ToDouble(r["FromAmount"]) & Allowance <= Convert.ToDouble(r["ToAmount"]))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string @type = r["type"].ToString();
                                if (type == "Percent")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0);
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employer"]) / 100), 0);
                                }
                                else if (type == "Amount")
                                {
                                    Employee = Convert.ToDouble(r["employee"]);
                                    Employer = Convert.ToDouble(r["employer"]);
                                }
                                if (OnIncome == "Yearly")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                    r["employee"] = Employee;
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                }
                            }
                            apply = true;
                        }
                        else if (Allowance >= Convert.ToDouble(r["FromAmount"]) && Convert.ToDouble(r["ToAmount"]) == 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string @type = r["type"].ToString();
                                if (type == "Percent")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0);
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employer"]) / 100), 0);
                                }
                                else if (type == "Amount")
                                {
                                    Employee = Convert.ToDouble(r["employee"]);
                                    Employer = Convert.ToDouble(r["employer"]);
                                }
                                if (OnIncome == "Yearly")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                    r["employee"] = Employee;
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                }
                            }
                            apply = true;
                        }

                        if (apply)
                        {
                            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeid;
                            cmd.Parameters.Add("@FromAmount", SqlDbType.Decimal, 10).Value = FromAmount;
                            cmd.Parameters.Add("@ToAmount", SqlDbType.Decimal, 10).Value = ToAmount;
                            cmd.Parameters.Add("@Employee", SqlDbType.Decimal, 10).Value = Employee;
                            cmd.Parameters.Add("@Employer", SqlDbType.Decimal, 10).Value = Employer;
                            cmd.Parameters.Add("@Amount", SqlDbType.Decimal, 10).Value = 0;
                            cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = r["type"];
                            cmd.Parameters.Add("@AllowanceID", SqlDbType.Int).Value = allowanceid;
                            cmd.Parameters.Add("@Fixed", SqlDbType.Decimal, 10).Value = r["Fixed"];
                            cmd.Parameters.Add("@DetailsID", SqlDbType.Int).Value = r["DetailsID"];

                            string insert = "INSERT INTO SlabDeduction(EmployeeID,FromAmount,ToAmount,Employee,Employer," +
                            "Amount,Type,AllowanceID, Fixed, DetailsID) " +
                            "VALUES( @EmployeeID, @FromAmount, @ToAmount, @Employee, @Employer, @Amount, @Type, @AllowanceID," +
                            "@Fixed, @DetailsID)";

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = insert;
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
            }
            return Content("Success", "text/plain");
        }
        public ContentResult  SetupDeduction(int employeeid, int categoryid, int deductionid) {

            {
                var cmd = new SqlCommand();
                var dtEMp = new DataTable();
                var dt = new DataTable();

                cmd.CommandText = "select EMployeeID from EMployee where CategoryID=" + categoryid;

                
                    // Open the connection from the DbContext
                    var connection = _context.Database.GetDbConnection(); // Get the DB connection from the DbContext

                    // Ensure the connection is open before executing the query
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();  // Open the connection
                    }

                    // Set the command properties
                    cmd.CommandType = CommandType.Text;  // Command type is SQL text
                    cmd.Connection = (SqlConnection)connection;  // Set the connection to the command

                    // Example query (replace with actual SQL query)
                    cmd.CommandText = "SELECT * FROM Employee";  // Replace with your actual query

                    // Create a SqlDataAdapter to execute the command
                    using (var sda = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataTable with the query result
                        sda.Fill(dtEMp);
                    }


                    var Employee = default(double);
                var Employer = default(double);
                
                    cmd.CommandText = "select distinct G.AllowanceID ,G.Employer,OnIncome from Deduction A  " + 
                        "Inner join Slab B On A.DeductionID =B.DeductionID  " + 
                        "inner join SlabDetails C On C.SlabID =B.SlabID " + 
                        "inner join Category D on C.CategoryID =D.CategoryID " + 
                        "inner Join SlabCalculation E on C.DetailsID = E.DetailsID and B.SlabID =E.SlabID " + 
                        "inner join Allowance F On E.AllowanceID = F.AllowanceID " + 
                        "inner join SlabAllowance G on F.AllowanceID = G.AllowanceID  " + 
                        "where A.DeductionID = " + deductionid + " And C.CategoryID = " + categoryid + " " + 
                        "And G.EmployeeID = " + employeeid + "";

                using (var sda = new SqlDataAdapter(cmd))
                {
                    // Fill the DataTable with the query result
                    sda.Fill(dt);
                }

                double Allowance = 0d;

                    string OnIncome = "";

                    foreach (DataRow row in dt.Rows)
                    {

                        if (row["OnIncome"] == "Monthly")
                        {
                            Allowance += (double)row.ItemArray[1];
                        }
                        else if (row["OnIncome"] == "Yearly Income")
                        {
                            double value = Convert.ToDouble(row.ItemArray[1]);  // Convert to double
                            Allowance += value * 12;
                            OnIncome = "Yearly";
                        }
                    }

                    cmd.CommandText = "delete from SlabDeduction where EmployeeID=" + employeeid + "" +
                    "and deductionID=" + deductionid;

                    cmd.ExecuteNonQuery();

                    string Sql = "select * from Slab A " + 
                    "inner join SlabDetails B on A.SlabID =B.SlabID " + 
                    "inner join Deduction C On A.DeductionID =C.DeductionID " + 
                    "where A.DeductionID = " + deductionid + " And B.CategoryID = " + categoryid + " ";



                    cmd.CommandText = Sql;
                    using (var sda = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataTable with the query result
                        sda.Fill(dt);
                    }

                foreach (DataRow r in dt.Rows)
                    {
                        double value = Convert.ToDouble(r["FromAmount"]);
                        double FromAmount = value;
                        value = Convert.ToDouble(r["FromAmount"]);
                        double ToAmount = value;

                        bool apply = false;

                        if (Convert.ToDouble(r["FromAmount"]) == 0 & Convert.ToDouble(r["ToAmount"]) == 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string @type = r["type"].ToString();
                                if (type == "Percent")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0);
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employer"]) / 100), 0);
                                }
                                else if (type == "Amount")
                                {
                                    Employee =Convert.ToDouble(r["employee"]);
                                    Employer =Convert.ToDouble(r["employer"]);
                                }
                                if (OnIncome == "Yearly")
                                {
                                    Employee =  Math.Round(Allowance * Convert.ToDouble(r["employee"]) / 100, 0) / 12;
                                    r["employee"] = Employee;
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                }
                            }
                            apply = true;
                        }
                        else if (Allowance >= Convert.ToDouble(r["FromAmount"]) & Allowance <= Convert.ToDouble(r["ToAmount"]))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string @type = r["type"].ToString();
                                if (type == "Percent")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0);
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employer"]) / 100), 0);
                                }
                                else if (type == "Amount")
                                {
                                    Employee = Convert.ToDouble(r["employee"]);
                                    Employer = Convert.ToDouble(r["employer"]);
                                }
                                if (OnIncome == "Yearly")
                                {
                                    Employee =  Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                    r["employee"] = Employee;
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                }
                            }
                            apply = true;
                        }
                        else if (Allowance >= Convert.ToDouble(r["FromAmount"]) && Convert.ToDouble(r["ToAmount"]) == 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string @type = r["type"].ToString();
                                if (type == "Percent")
                                {
                                    Employee = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0);
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employer"]) / 100), 0);
                                }
                                else if (type == "Amount")
                                {
                                    Employee = Convert.ToDouble(r["employee"]);
                                    Employer = Convert.ToDouble(r["employer"]);
                                }
                                if (OnIncome == "Yearly")
                                {
                                    Employee =  Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                    r["employee"] = Employee;
                                    Employer = Math.Round(Allowance * (Convert.ToDouble(r["employee"]) / 100), 0) / 12;
                                }
                            }
                            apply = true;
                        }

                        if (apply)
                        {
                            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeid;
                            cmd.Parameters.Add("@FromAmount", SqlDbType.Decimal, 10).Value = FromAmount;
                            cmd.Parameters.Add("@ToAmount", SqlDbType.Decimal, 10).Value = ToAmount;
                            cmd.Parameters.Add("@Employee", SqlDbType.Decimal, 10).Value = Employee;
                            cmd.Parameters.Add("@Employer", SqlDbType.Decimal, 10).Value = Employer;
                            cmd.Parameters.Add("@Amount", SqlDbType.Decimal, 10).Value = 0;
                            cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = r["type"];
                            cmd.Parameters.Add("@DeductionID", SqlDbType.Int).Value = deductionid;
                            cmd.Parameters.Add("@Fixed", SqlDbType.Decimal, 10).Value = r["Fixed"];
                            cmd.Parameters.Add("@DetailsID", SqlDbType.Int).Value = r["DetailsID"];

                            string insert = "INSERT INTO SlabDeduction(EmployeeID,FromAmount,ToAmount,Employee,Employer," +
                            "Amount,Type,DeductionID, Fixed, DetailsID) " +
                            "VALUES( @EmployeeID, @FromAmount, @ToAmount, @Employee, @Employer, @Amount, @Type, @DeductionID," +
                            "@Fixed, @DetailsID)";

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = insert;
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
            


            return Content("Success","text/plain");
        
        }
        public async Task<IActionResult> Deduction(int employeeid, int categoryid)
        {

            var deduction = from A in _context.Deduction
                         join B in _context.Slab on A.ID equals B.DeductionID
                         join C in _context.SlabCategory on B.SlabID equals C.SlabID
                            join D in _context.SlabCalculation on B.SlabID equals D.SlabID
                            join E in _context.SlabDetails on D.DetailsID equals E.ID
                            where C.CategoryID == (categoryid == null ? 0 : Convert.ToInt32(categoryid))
                            && B.SlabID == E.SlabID
                            orderby A.PayRollSlNo
                            select new DeductionDetails
                            {
                                ID = A.ID,
                                Name = A.Name,
                                Amount = 0,
                                Fixed = A.Fixed == null ? false : true,
                                FromAmount = 0,
                                ToAmount = 0,
                                Employee = 0,
                                Employer = 0,
                                DetailsID = E.ID
                            };

            var slab = from A in _context.SlabDeduction
                            join B in _context.Deduction on A.DeductionID equals B.ID
                            where A.EmployeeID == (employeeid == null ? 0 : Convert.ToInt32(employeeid))
                            orderby B.PayRollSlNo
                            select new
                            {
                                A.DeductionID,
                                B.Name,
                                Amount = 0,
                                B.Fixed,
                                A.FromAmount,
                                A.ToAmount,
                                A.Employee,
                                A.Employer,
                                A.Type,
                                A.DetailsID
                            };

            // Merging the two lists based on AllowanceID
            var deduction_set = deduction.ToList().Join(slab,
                                d => d.ID,
                                s => s.DeductionID,
                                (d, s) =>
                                {
                                    d.Amount = s.Amount;
                                    d.Fixed = s.Fixed == null ? false : true;
                                    d.FromAmount = s.FromAmount;
                                    d.ToAmount = s.ToAmount;
                                    d.Employee = s.Employee;
                                    d.Employer = s.Employer;
                                    return d;
                                }).ToList();

            return View(deduction_set);
        }
    }
}
