using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rpsls.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WinnerId = table.Column<int>(type: "int", nullable: false),
                    LoserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRules_Choices_LoserId",
                        column: x => x.LoserId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameRules_Choices_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Choices",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rock" },
                    { 2, "Paper" },
                    { 3, "Scissors" },
                    { 4, "Lizard" },
                    { 5, "Spock" }
                });

            migrationBuilder.InsertData(
                table: "GameRules",
                columns: new[] { "Id", "Description", "LoserId", "WinnerId" },
                values: new object[,]
                {
                    { 1, "Rock crushes scissors.", 3, 1 },
                    { 2, "Rock crushes lizard.", 4, 1 },
                    { 3, "Paper covers rock.", 1, 2 },
                    { 4, "Paper disproves Spock.", 5, 2 },
                    { 5, "Scissors cuts paper.", 2, 3 },
                    { 6, "Scissors decapitates lizard.", 4, 3 },
                    { 7, "Lizard poisons Spock.", 5, 4 },
                    { 8, "Lizard eats paper.", 2, 4 },
                    { 9, "Spock smashes scissors.", 3, 5 },
                    { 10, "Spock vaporizes rock.", 1, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRules_LoserId",
                table: "GameRules",
                column: "LoserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRules_WinnerId",
                table: "GameRules",
                column: "WinnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRules");

            migrationBuilder.DropTable(
                name: "Choices");
        }
    }
}
