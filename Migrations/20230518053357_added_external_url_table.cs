using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_api.Migrations
{
    /// <inheritdoc />
    public partial class added_external_url_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalUrls",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "ExternalUrlsId",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "User",
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
                name: "IX_User_ExternalUrlsId",
                table: "User",
                column: "ExternalUrlsId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ExternalUrls_ExternalUrlsId",
                table: "User",
                column: "ExternalUrlsId",
                principalTable: "ExternalUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ExternalUrls_ExternalUrlsId",
                table: "User");

            migrationBuilder.DropTable(
                name: "ExternalUrls");

            migrationBuilder.DropIndex(
                name: "IX_User_ExternalUrlsId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ExternalUrlsId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrls",
                table: "User",
                type: "TEXT",
                nullable: true);
        }
    }
}
