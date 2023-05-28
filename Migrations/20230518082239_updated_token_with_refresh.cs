﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_api.Migrations
{
    /// <inheritdoc />
    public partial class updated_token_with_refresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "SpotifyToken",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "SpotifyToken");
        }
    }
}
