using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PUZZLEBOX;

#nullable disable

namespace SKELETON_KING.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(BountyContext))]
    [Migration("20230726002351_ImportBountyDatabaseSchema")]
    public partial class ImportBountyDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This will grant a 
            // File property for sql file  = Always copy
            var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations");
            var path = Path.Combine(baseDirectory, "20230726002351_ImportBountyDatabaseSchema.sql");
            migrationBuilder.Sql(File.ReadAllText(path));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
      
        }
    }
}