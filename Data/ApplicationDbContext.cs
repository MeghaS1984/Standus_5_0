using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.AppSetup.Models;
using Standus_5_0.Areas.HumanResource.Models;
using System.Reflection.Emit;

namespace Standus_5_0.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
                
        public DbSet<Employee> Employee { get; set; } = default!;
        public DbSet<Department> Department { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Education> Education { get; set; } = default!;
        public DbSet<EmploymentHistory> EmploymentHistory { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Family> Family { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.PerformanceEvaluation> PerformanceEvaluation { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Position> Position { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Statutory> Statutory { get; set; } = default!;
        //public DbSet<Standus_5_0.Areas.HumanResource.Models.EmployementDetails> EmployementDetails { get; set; } = default!;
        //public DbSet<Standus_5_0.Areas.HumanResource.Models.BankDetails> BankDetails { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This configures the inheritance with a discriminator column to distinguish Employee and BankDetails
            //modelBuilder.Entity<Employee>()
            //    .HasDiscriminator<string>("Discriminator")
            //    .HasValue<Employee>("Employee")
            //    .HasValue<EmployementDetails>("EmployementDetails")
            //    .HasValue<BankDetails>("BankDetails");

            modelBuilder.Entity<EmployementDetails>();
            modelBuilder.Entity<BankDetails>();
            modelBuilder.Entity<Resigned>();

            modelBuilder.Entity<SlabCategory>().HasKey(ck => new { ck.SlabID ,ck.CategoryID});
            modelBuilder.Entity<StandardDeduction>().HasKey(ck => new  { ck.EmployeeID, ck.DeductionID });
            modelBuilder.Entity<StandardDeductionCalculation>().HasKey(ck => new { ck.DeductionID , ck.AllowanceID });
            modelBuilder.Entity<FriengeAssign>().HasKey(ck => new { ck.FriengeID , ck.EmployeeID });
            modelBuilder.Entity<LoanSchedule>().HasKey(ck => new { ck.SanctionID });
            modelBuilder.Entity<SlabDeductionExclude>().HasKey(ck => new { ck.EmployeeID , ck.DeductionID });
            modelBuilder.Entity<IncentiveSetting>().HasKey(ck => new { ck.EmployeeID });
            
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Allowance> Allowance { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.AttendanceHead> AttendanceHead { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.AppSetup.Models.Category> Category { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Deduction> Deduction { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.AppSetup.Models.Designation> Designation { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Grade> Grade { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Holidays> Holidays { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.LeaveApplication> LeaveApplication { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.LeaveAllocation> LeaveAllocation { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.AppSetup.Models.Menu> Menu { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Slab> Slab { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.SlabCategory> SlabCategory { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.SlabDetails> SlabDetails { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.StandardDeduction> StandardDeduction { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.StandardDeductionCalculation> StandardDeductionCalculation { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Frienge> Frienge { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.FriengeAssign> FriengeAssign { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.LoanRequest> LoanRequest { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.LoanSanction> LoanSanction { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.LoanSchedule> LoanSchedule { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.LoanForward> LoanForward { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.SlabDeductionExclude> SlabDeductionExclude { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.IncentiveSetting> IncentiveSetting { get; set; } = default!;
        public DbSet<Standus_5_0.Areas.HumanResource.Models.Payroll> Payroll { get; set; } = default!;
        public DbSet<PaySheetSetting> paySheetSetting { get; set; } = default!;

        public DbSet<PaySheetSummarySetting> PaySheetSummarySetting { get; set; } = default!;
        public DbSet<EAndDSetting> EAndDSetting { get; set; }
        public DbSet<EAndDSettingParam> EAndDSettingParam { get; set; }
    }

    
}
