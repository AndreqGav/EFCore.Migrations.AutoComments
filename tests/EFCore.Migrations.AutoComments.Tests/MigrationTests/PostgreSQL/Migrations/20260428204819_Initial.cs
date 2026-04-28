using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCore.Migrations.AutoComments.Tests.MigrationTests.PostgreSQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор блога.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true, comment: "Название блога."),
                    Url = table.Column<string>(type: "text", nullable: true, comment: "URL блога.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                },
                comment: "Блог.");

            migrationBuilder.CreateTable(
                name: "BlogViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true, comment: "Наименование."),
                    Url = table.Column<string>(type: "text", nullable: true, comment: "URL.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogViews", x => x.Id);
                },
                comment: "Представление блога.");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор заказа.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true, comment: "Номер заказа."),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false, comment: "Итоговая сумма заказа в рублях."),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "Статус подтверждения заказа."),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Статус заказа.\n\n0 - Активный, ожидает выполнения.\n1 - Выполнен, доставлен покупателю.\n2 - Отменён, возврат средств."),
                    Category = table.Column<int>(type: "integer", nullable: false, comment: "Категория заказа.\n\n0 - Одежда.\n1 - Книги.\n2 - Игрушки."),
                    DeliveryMethod = table.Column<int>(type: "integer", nullable: false, comment: "Способ доставки.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                },
                comment: "Заказ покупателя.");

            migrationBuilder.CreateTable(
                name: "PostBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    TextA = table.Column<string>(type: "text", nullable: true, comment: "Текст А."),
                    TextB = table.Column<string>(type: "text", nullable: true, comment: "Текст Б.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBase", x => x.Id);
                },
                comment: "Базовый тип в наследовании TPH.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "BlogViews");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PostBase");
        }
    }
}
