using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SKELETON_KING.Migrations
{
    /// <inheritdoc />
    public partial class CloudStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CloudStorages",
                columns: table => new
                {
                    CloudStorageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    UseCloud = table.Column<bool>(type: "bit", nullable: false),
                    CloudAutoupload = table.Column<bool>(type: "bit", nullable: false),
                    FileModifyTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CloudCfgZip = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudStorages", x => x.CloudStorageId);
                    table.ForeignKey(
                        name: "FK_CloudStorages_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudStorages_AccountId",
                table: "CloudStorages",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudStorages");
        }
    }
}
