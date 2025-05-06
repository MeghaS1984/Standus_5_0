using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Standus_5_0.Data
{
    
    public class GetData
    {
        private SqlCommand cmd = new SqlCommand();
        private readonly string _cs;

        public GetData(string cs)
        {
            _cs = cs;
        }
       
            public DataTable Data(string sql, string server = "")
        {
            //string cs;
            //if (server == "")
            //    cs = _context.Database.GetDbConnection().ConnectionString;
            //else
            //    cs = _context.Database.GetDbConnection().ConnectionString;
            SqlConnection conn = new SqlConnection(_cs);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            return dt;
        }
    }

}
