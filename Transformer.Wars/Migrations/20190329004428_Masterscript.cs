using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transformer.Wars.Migrations
{
    public partial class Masterscript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllegianceTypes",
                columns: table => new
                {
                    AllegianceTypeId = table.Column<int>(nullable: false),
                    AllegianceTypeTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllegianceTypes", x => x.AllegianceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Transformers",
                columns: table => new
                {
                    AllegianceTypeId = table.Column<int>(nullable: false),
                    TransformerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    Rank = table.Column<short>(nullable: false),
                    Strength = table.Column<short>(nullable: false),
                    Intelligence = table.Column<short>(nullable: false),
                    Speed = table.Column<short>(nullable: false),
                    Endurance = table.Column<short>(nullable: false),
                    Courage = table.Column<short>(nullable: false),
                    Firepower = table.Column<short>(nullable: false),
                    Skill = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transformers", x => x.AllegianceTypeId);
                    table.UniqueConstraint("AK_Transformers_TransformerId", x => x.TransformerId);
                    table.ForeignKey(
                        name: "FK_Transformers_AllegianceTypes_AllegianceTypeId",
                        column: x => x.AllegianceTypeId,
                        principalTable: "AllegianceTypes",
                        principalColumn: "AllegianceTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transformers_AllegianceTypeId",
                table: "Transformers",
                column: "AllegianceTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transformers");

            migrationBuilder.DropTable(
                name: "AllegianceTypes");
        }
    }
}
