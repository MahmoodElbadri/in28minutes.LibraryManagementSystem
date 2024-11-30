using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace in28minutes.Library.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seeding Categories
            var categoryGuid1 = Guid.NewGuid();
            var categoryGuid2 = Guid.NewGuid();

            migrationBuilder.InsertData(
            table: "Categories",
            columns: new[] { "Id", "Name", "Description", "CreatedAt", "UpdatedAt" }, // Include "UpdatedAt"
            values: new object[,]
            {
                { categoryGuid1, "Science Fiction", "A genre of speculative fiction.", DateTime.Now, DateTime.Now },
                { categoryGuid2, "Fantasy", "A genre of speculative fiction involving magic.", DateTime.Now, DateTime.Now }
            });


            // Seeding Books
            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "Title", "Author", "ISBN", "Publisher", "PublishDate", "Description", "CopiesAvailable", "TotalCopies", "CategoryId", "CreatedAt", "UpdatedAt" },
            values: new object[,]
            {
                {
                    Guid.NewGuid(), "Dune", "Frank Herbert", "9780441013593", "Ace",
                    new DateTime(1965, 8, 1), "A science fiction novel.", true, 5,
                    categoryGuid1, DateTime.Now, DateTime.Now
                },
                {
                    Guid.NewGuid(), "The Hobbit", "J.R.R. Tolkien", "9780345339683", "Houghton Mifflin",
                    new DateTime(1937, 9, 21), "A fantasy novel.", true, 7,
                    categoryGuid2, DateTime.Now, DateTime.Now
                }
            });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // To revert the changes, we can delete the inserted rows
            migrationBuilder.DeleteData(
                table: "Books", // Name of the table
                keyColumn: "Id", // Primary key column
                keyValues: new object[] { /* list of primary keys of the books added */ });

            migrationBuilder.DeleteData(
                table: "Categories", // Name of the table
                keyColumn: "Id", // Primary key column
                keyValues: new object[] { /* list of primary keys of the categories added */ });
        }
    }
}
