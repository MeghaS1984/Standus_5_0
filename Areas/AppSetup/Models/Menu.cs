using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Standus_5_0.Areas.AppSetup.Models
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int ID { get; set; }
        public string? GroupName { get; set; }
        public string? MenuName { get; set; }
        public string? UrlPath { get; set; }
        public string? SubGroup { get; set; }
    }

}
