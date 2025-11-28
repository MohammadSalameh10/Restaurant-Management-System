using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOps.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class convertorderstatustoenum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "OrderStatusEnum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderStatusEnum",
                table: "Orders",
                newName: "Status");
        }
    }
}
