using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCore.Migrations.AutoComments.Tests.MigrationTests.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "BlogBaseSequence");

            migrationBuilder.CreateTable(
                name: "ArticleBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleBase", x => x.Id);
                },
                comment: "Base type in TPT inheritance.");

            migrationBuilder.CreateTable(
                name: "BlogA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"BlogBaseSequence\"')", comment: "Identifier."),
                    Name = table.Column<string>(type: "text", nullable: true, comment: "Name A.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogA", x => x.Id);
                },
                comment: "TPC derived type A.");

            migrationBuilder.CreateTable(
                name: "BlogB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"BlogBaseSequence\"')", comment: "Identifier."),
                    Name = table.Column<string>(type: "text", nullable: true, comment: "Name B.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogB", x => x.Id);
                },
                comment: "TPC derived type B.");

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Blog identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true, comment: "Blog name."),
                    Url = table.Column<string>(type: "text", nullable: true, comment: "Blog URL.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                },
                comment: "Blog.");

            migrationBuilder.CreateTable(
                name: "BlogViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true, comment: "Name."),
                    Url = table.Column<string>(type: "text", nullable: true, comment: "URL.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogViews", x => x.Id);
                },
                comment: "Blog view.");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Order identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true, comment: "Order number."),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false, comment: "Total order amount."),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "Order confirmation status."),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Order status.\n\n0 - Active, awaiting fulfillment.\n1 - Completed, delivered to the customer.\n2 - Cancelled, refund issued."),
                    Category = table.Column<int>(type: "integer", nullable: false, comment: "Order category.\n\n0 - Clothing.\n1 - Books.\n2 - Toys."),
                    DeliveryMethod = table.Column<int>(type: "integer", nullable: false, comment: "Delivery method.")
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
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    TextA = table.Column<string>(type: "text", nullable: true, comment: "Text A."),
                    TextB = table.Column<string>(type: "text", nullable: true, comment: "Text B.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBase", x => x.Id);
                },
                comment: "Base type in TPH inheritance.");

            migrationBuilder.CreateTable(
                name: "ArticleA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Identifier."),
                    ContentA = table.Column<string>(type: "text", nullable: true, comment: "Type A specific content.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleA_ArticleBase_Id",
                        column: x => x.Id,
                        principalTable: "ArticleBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "TPT derived type A.");

            migrationBuilder.CreateTable(
                name: "ArticleB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Identifier."),
                    ContentB = table.Column<string>(type: "text", nullable: true, comment: "Type B specific content.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleB_ArticleBase_Id",
                        column: x => x.Id,
                        principalTable: "ArticleBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "TPT derived type B.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleA");

            migrationBuilder.DropTable(
                name: "ArticleB");

            migrationBuilder.DropTable(
                name: "BlogA");

            migrationBuilder.DropTable(
                name: "BlogB");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "BlogViews");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PostBase");

            migrationBuilder.DropTable(
                name: "ArticleBase");

            migrationBuilder.DropSequence(
                name: "BlogBaseSequence");
        }
    }
}
