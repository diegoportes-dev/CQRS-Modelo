using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuProjeto.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CategoriasEProdutoCategoriaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoriaId",
                table: "Produtos",
                type: "char(36)",
                nullable: true);

            migrationBuilder.Sql(
                """
                INSERT INTO `Categorias` (`Id`, `Descricao`)
                SELECT UUID(), categorias_existentes.`Categoria`
                FROM (
                    SELECT DISTINCT `Categoria`
                    FROM `Produtos`
                ) AS categorias_existentes;
                """);

            migrationBuilder.Sql(
                """
                UPDATE `Produtos` AS produto
                INNER JOIN `Categorias` AS categoria
                    ON categoria.`Descricao` = produto.`Categoria`
                SET produto.`CategoriaId` = categoria.`Id`;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoriaId",
                table: "Produtos",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_CategoriaId",
                table: "Produtos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Produtos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categorias_CategoriaId",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Produtos",
                type: "longtext",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE `Produtos` AS produto
                INNER JOIN `Categorias` AS categoria
                    ON categoria.`Id` = produto.`CategoriaId`
                SET produto.`Categoria` = categoria.`Descricao`;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "Produtos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
