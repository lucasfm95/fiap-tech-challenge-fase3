using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fiap.TechChallenge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDddStateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "DddNumber",
                table: "Contacts",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "DddState",
                columns: table => new
                {
                    DddNumber = table.Column<short>(type: "smallint", maxLength: 3, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StateAbbreviation = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DddState", x => x.DddNumber);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_DddNumber",
                table: "Contacts",
                column: "DddNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_DddState_DddNumber",
                table: "Contacts",
                column: "DddNumber",
                principalTable: "DddState",
                principalColumn: "DddNumber",
                onDelete: ReferentialAction.NoAction);
            
            
            migrationBuilder.InsertData(
                table: "DddState",
                columns: new[] { "DddNumber", "StateName", "StateAbbreviation" },
                values: new object[,]
                {
                    { "68", "Acre", "AC" },
                    { "82", "Alagoas", "AL" },
                    { "97", "Amazonas", "AM" },
                    { "92", "Amazonas", "AM" },
                    { "96", "Amapá", "AP" },
                    { "77", "Bahia", "BA" },
                    { "75", "Bahia", "BA" },
                    { "73", "Bahia", "BA" },
                    { "74", "Bahia", "BA" },
                    { "71", "Bahia", "BA" },
                    { "88", "Ceará", "CE" },
                    { "85", "Ceará", "CE" },
                    { "61", "Distrito Federal", "DF" },
                    { "28", "Espírito Santo", "ES" },
                    { "27", "Espírito Santo", "ES" },
                    { "62", "Goiás", "GO" },
                    { "64", "Goiás", "GO" },
                    { "99", "Maranhão", "MA" },
                    { "98", "Maranhão", "MA" },
                    { "34", "Minas Gerais", "MG" },
                    { "37", "Minas Gerais", "MG" },
                    { "31", "Minas Gerais", "MG" },
                    { "33", "Minas Gerais", "MG" },
                    { "35", "Minas Gerais", "MG" },
                    { "38", "Minas Gerais", "MG" },
                    { "32", "Minas Gerais", "MG" },
                    { "67", "Mato Grosso do Sul", "MS" },
                    { "65", "Mato Grosso", "MT" },
                    { "66", "Mato Grosso", "MT" },
                    { "91", "Pará", "PA" },
                    { "94", "Pará", "PA" },
                    { "93", "Pará", "PA" },
                    { "83", "Paraíba", "PB" },
                    { "81", "Pernambuco", "PE" },
                    { "87", "Pernambuco", "PE" },
                    { "89", "Piauí", "PI" },
                    { "86", "Piauí", "PI" },
                    { "43", "Paraná", "PR" },
                    { "41", "Paraná", "PR" },
                    { "42", "Paraná", "PR" },
                    { "44", "Paraná", "PR" },
                    { "45", "Paraná", "PR" },
                    { "46", "Paraná", "PR" },
                    { "24", "Rio de Janeiro", "RJ" },
                    { "22", "Rio de Janeiro", "RJ" },
                    { "21", "Rio de Janeiro", "RJ" },
                    { "84", "Rio Grande do Norte", "RN" },
                    { "53", "Rio Grande do Sul", "RS" },
                    { "54", "Rio Grande do Sul", "RS" },
                    { "55", "Rio Grande do Sul", "RS" },
                    { "51", "Rio Grande do Sul", "RS" },
                    { "47", "Santa Catarina", "SC" },
                    { "48", "Santa Catarina", "SC" },
                    { "49", "Santa Catarina", "SC" },
                    { "79", "Sergipe", "SE" },
                    { "11", "São Paulo", "SP" },
                    { "12", "São Paulo", "SP" },
                    { "13", "São Paulo", "SP" },
                    { "14", "São Paulo", "SP" },
                    { "15", "São Paulo", "SP" },
                    { "16", "São Paulo", "SP" },
                    { "17", "São Paulo", "SP" },
                    { "18", "São Paulo", "SP" },
                    { "19", "São Paulo", "SP" },
                    { "63", "Tocantins", "TO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_DddState_DddNumber",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "DddState");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_DddNumber",
                table: "Contacts");

            migrationBuilder.AlterColumn<int>(
                name: "DddNumber",
                table: "Contacts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}
