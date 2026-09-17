using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfinityPart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAutenticacaoAdministrador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenhaHash",
                table: "Administradores",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Administradores",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoAcesso",
                table: "Administradores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Administradores_Email",
                table: "Administradores",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Administradores_Email",
                table: "Administradores");

            migrationBuilder.DropColumn(
                name: "SenhaHash",
                table: "Administradores");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Administradores");

            migrationBuilder.DropColumn(
                name: "UltimoAcesso",
                table: "Administradores");
        }
    }
}
