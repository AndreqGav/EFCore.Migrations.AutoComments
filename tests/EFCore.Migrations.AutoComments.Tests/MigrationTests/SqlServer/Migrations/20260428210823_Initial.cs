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
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор блога.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Название блога."),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "URL блога.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                },
                comment: "Блог.");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор заказа.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Номер заказа."),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Итоговая сумма заказа в рублях."),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false, comment: "Статус подтверждения заказа."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Статус заказа.\n\n0 - Активный, ожидает выполнения.\n1 - Выполнен, доставлен покупателю.\n2 - Отменён, возврат средств."),
                    Category = table.Column<int>(type: "int", nullable: false, comment: "Категория заказа."),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false, comment: "Способ доставки.")
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
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextA = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Текст А."),
                    TextB = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Текст Б.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBase", x => x.Id);
                },
                comment: "Базовый тип в наследовании TPH.");
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
