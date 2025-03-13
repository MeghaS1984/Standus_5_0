using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standus_5_0.Migrations.Allowance
{
    /// <inheritdoc />
    public partial class Allowance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name:"AccountID",
            //    table:"Allowance",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldNullable:false);

            //migrationBuilder.AlterColumn<int>(
            //    name: "DebitTo",
            //    table: "Allowance",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldNullable: false);

            //migrationBuilder.AlterColumn<int>(
            //    name: "CreditTo",
            //    table: "Allowance",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldNullable: false);

            migrationBuilder.AlterColumn<Boolean>(
                name: "AuthorisedLeave",
                table: "Allowance",
                nullable: true,
                oldClrType: typeof(Boolean),
                oldNullable: false);


            //migrationBuilder.CreateTable(
            //    name: "Allowance",
            //    columns: table => new
            //    {
            //        AllowanceID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        CutOffType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Period = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DaysPresent = table.Column<bool>(type: "bit", nullable: true),
            //        AuthorisedLeave = table.Column<bool>(type: "bit", nullable: true),
            //        GeneralHoliday = table.Column<bool>(type: "bit", nullable: true),
            //        CutOff = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        RoundOf = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AccountID = table.Column<int>(type: "int", nullable: true),
            //        Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Day = table.Column<int>(type: "int", nullable: false),
            //        Variable = table.Column<int>(type: "int", nullable: false),
            //        PayrollSlNO = table.Column<int>(type: "int", nullable: false),
            //        InActive = table.Column<bool>(type: "bit", nullable: false),
            //        Fixed = table.Column<bool>(type: "bit", nullable: false),
            //        DebitTo = table.Column<int>(type: "int", nullable: true),
            //        CreditTo = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Allowance", x => x.AllowanceID);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allowance");
        }
    }
}
