using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactRegister.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEntidadeDdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_contact_tb_ddd_ddd_id",
                table: "tb_contact");

            migrationBuilder.DropTable(
                name: "tb_ddd");

            migrationBuilder.DropIndex(
                name: "IX_tb_contact_ddd_id",
                table: "tb_contact");

            migrationBuilder.RenameColumn(
                name: "ddd_id",
                table: "tb_contact",
                newName: "ddd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ddd",
                table: "tb_contact",
                newName: "ddd_id");

            migrationBuilder.CreateTable(
                name: "tb_ddd",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ddd", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_contact_ddd_id",
                table: "tb_contact",
                column: "ddd_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ddd_code",
                table: "tb_ddd",
                column: "code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_contact_tb_ddd_ddd_id",
                table: "tb_contact",
                column: "ddd_id",
                principalTable: "tb_ddd",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
