using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddRecommendationMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMovie",
                table: "UserMovie");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserMovie",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "UserMovie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientNotes",
                table: "UserMovie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipientRating",
                table: "UserMovie",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedByUserId",
                table: "UserMovie",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMovie",
                table: "UserMovie",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovie_AppUserId_MovieId",
                table: "UserMovie",
                columns: new[] { "AppUserId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMovie_RecommendedByUserId",
                table: "UserMovie",
                column: "RecommendedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovie_AspNetUsers_RecommendedByUserId",
                table: "UserMovie",
                column: "RecommendedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovie_AspNetUsers_RecommendedByUserId",
                table: "UserMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMovie",
                table: "UserMovie");

            migrationBuilder.DropIndex(
                name: "IX_UserMovie_AppUserId_MovieId",
                table: "UserMovie");

            migrationBuilder.DropIndex(
                name: "IX_UserMovie_RecommendedByUserId",
                table: "UserMovie");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserMovie");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "UserMovie");

            migrationBuilder.DropColumn(
                name: "RecipientNotes",
                table: "UserMovie");

            migrationBuilder.DropColumn(
                name: "RecipientRating",
                table: "UserMovie");

            migrationBuilder.DropColumn(
                name: "RecommendedByUserId",
                table: "UserMovie");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMovie",
                table: "UserMovie",
                columns: new[] { "AppUserId", "MovieId" });
        }
    }
}
