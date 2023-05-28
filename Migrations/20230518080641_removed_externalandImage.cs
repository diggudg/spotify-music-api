using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_api.Migrations
{
    /// <inheritdoc />
    public partial class removed_externalandImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ExternalUrls_ExternalUrlsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ExternalUrls");

            migrationBuilder.DropIndex(
                name: "IX_Users_ExternalUrlsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ExternalUrlsId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalUrlsId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExternalUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Spotify = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalUrlsId",
                table: "Users",
                column: "ExternalUrlsId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                table: "Images",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ExternalUrls_ExternalUrlsId",
                table: "Users",
                column: "ExternalUrlsId",
                principalTable: "ExternalUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
