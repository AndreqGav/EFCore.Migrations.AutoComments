using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Migrations.AutoComments.Tests.MigrationTests.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Blog identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Blog name."),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Blog URL.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                },
                comment: "Blog.");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Order identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Order number."),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total order amount."),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false, comment: "Order confirmation status."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Order status.\n\n0 - Active, awaiting fulfillment.\n1 - Completed, delivered to the customer.\n2 - Cancelled, refund issued."),
                    Category = table.Column<int>(type: "int", nullable: false, comment: "Order category."),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false, comment: "Delivery method.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                },
                comment: "Customer order.");

            migrationBuilder.CreateTable(
                name: "PostBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    TextA = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Text A."),
                    TextB = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Text B.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBase", x => x.Id);
                },
                comment: "Base type in TPH inheritance.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PostBase");
        }
    }
}
