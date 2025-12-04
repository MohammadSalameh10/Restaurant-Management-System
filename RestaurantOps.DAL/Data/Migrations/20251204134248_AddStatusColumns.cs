using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOps.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Customers', 'Status') IS NULL
    ALTER TABLE [Customers] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Employees', 'Status') IS NULL
    ALTER TABLE [Employees] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('InventoryItems', 'Status') IS NULL
    ALTER TABLE [InventoryItems] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('MenuItems', 'Status') IS NULL
    ALTER TABLE [MenuItems] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('MenuItemIngredients', 'Status') IS NULL
    ALTER TABLE [MenuItemIngredients] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Orders', 'Status') IS NULL
    ALTER TABLE [Orders] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('OrderTypes', 'Status') IS NULL
    ALTER TABLE [OrderTypes] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('OrderItems', 'Status') IS NULL
    ALTER TABLE [OrderItems] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Payments', 'Status') IS NULL
    ALTER TABLE [Payments] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Suppliers', 'Status') IS NULL
    ALTER TABLE [Suppliers] ADD [Status] int NOT NULL DEFAULT 1;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Locations', 'Status') IS NULL
    ALTER TABLE [Locations] ADD [Status] int NOT NULL DEFAULT 1;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Customers', 'Status') IS NOT NULL
    ALTER TABLE [Customers] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Employees', 'Status') IS NOT NULL
    ALTER TABLE [Employees] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('InventoryItems', 'Status') IS NOT NULL
    ALTER TABLE [InventoryItems] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('MenuItems', 'Status') IS NOT NULL
    ALTER TABLE [MenuItems] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('MenuItemIngredients', 'Status') IS NOT NULL
    ALTER TABLE [MenuItemIngredients] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Orders', 'Status') IS NOT NULL
    ALTER TABLE [Orders] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('OrderTypes', 'Status') IS NOT NULL
    ALTER TABLE [OrderTypes] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('OrderItems', 'Status') IS NOT NULL
    ALTER TABLE [OrderItems] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Payments', 'Status') IS NOT NULL
    ALTER TABLE [Payments] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Suppliers', 'Status') IS NOT NULL
    ALTER TABLE [Suppliers] DROP COLUMN [Status];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Locations', 'Status') IS NOT NULL
    ALTER TABLE [Locations] DROP COLUMN [Status];
");
        }
    }
}
