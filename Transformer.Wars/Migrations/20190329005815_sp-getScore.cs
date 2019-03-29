using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transformer.Wars.Migrations
{
    public partial class spgetScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[GetScore]
                    @TransformerId int
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT (Strength + Intelligence + Speed + Endurance + Courage + Firepower + Skill) as Score FROM Transformers Where TransformerId = @TransformerId
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[GetScore]";
            migrationBuilder.Sql(sp);
        }
    }
}
