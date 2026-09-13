using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaksymKostyshyn.MindTrack.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class LinkReflectionAnswersToJournalEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReflectionAnswers_JournalEntryId",
                table: "ReflectionAnswers",
                column: "JournalEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReflectionAnswers_JournalEntry_JournalEntryId",
                table: "ReflectionAnswers",
                column: "JournalEntryId",
                principalTable: "JournalEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReflectionAnswers_JournalEntry_JournalEntryId",
                table: "ReflectionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ReflectionAnswers_JournalEntryId",
                table: "ReflectionAnswers");
        }
    }
}
