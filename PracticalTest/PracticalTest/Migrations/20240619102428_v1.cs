using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticalTest.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "basicDetails",
                columns: table => new
                {
                    BasicDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basicDetails", x => x.BasicDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "educations",
                columns: table => new
                {
                    EducationDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoardUniversity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CGPAOrPercentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_educations", x => x.EducationDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "experiences",
                columns: table => new
                {
                    WorkExperiencesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experiences", x => x.WorkExperiencesId);
                });

            migrationBuilder.CreateTable(
                name: "LanguageDetails",
                columns: table => new
                {
                    LanguagesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanRead = table.Column<bool>(type: "bit", nullable: false),
                    CanWrite = table.Column<bool>(type: "bit", nullable: false),
                    CanSpeak = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageDetails", x => x.LanguagesId);
                });

            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    PreferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreferredLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedCTC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentCTC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoticePeriod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.PreferenceId);
                });

            migrationBuilder.CreateTable(
                name: "techExpertises",
                columns: table => new
                {
                    TechnicalExperiencesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Technology = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProficiencyLevel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_techExpertises", x => x.TechnicalExperiencesId);
                });

            migrationBuilder.CreateTable(
                name: "applicationForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasicDetailsId = table.Column<int>(type: "int", nullable: false),
                    EducationDetailsId = table.Column<int>(type: "int", nullable: false),
                    WorkExperiencesId = table.Column<int>(type: "int", nullable: false),
                    LanguagesId = table.Column<int>(type: "int", nullable: false),
                    TechnicalExperiencesId = table.Column<int>(type: "int", nullable: false),
                    PreferenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_applicationForms_basicDetails_BasicDetailsId",
                        column: x => x.BasicDetailsId,
                        principalTable: "basicDetails",
                        principalColumn: "BasicDetailsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applicationForms_educations_EducationDetailsId",
                        column: x => x.EducationDetailsId,
                        principalTable: "educations",
                        principalColumn: "EducationDetailsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applicationForms_experiences_WorkExperiencesId",
                        column: x => x.WorkExperiencesId,
                        principalTable: "experiences",
                        principalColumn: "WorkExperiencesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applicationForms_LanguageDetails_LanguagesId",
                        column: x => x.LanguagesId,
                        principalTable: "LanguageDetails",
                        principalColumn: "LanguagesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applicationForms_Preference_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "Preference",
                        principalColumn: "PreferenceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applicationForms_techExpertises_TechnicalExperiencesId",
                        column: x => x.TechnicalExperiencesId,
                        principalTable: "techExpertises",
                        principalColumn: "TechnicalExperiencesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_applicationForms_BasicDetailsId",
                table: "applicationForms",
                column: "BasicDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_applicationForms_EducationDetailsId",
                table: "applicationForms",
                column: "EducationDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_applicationForms_LanguagesId",
                table: "applicationForms",
                column: "LanguagesId");

            migrationBuilder.CreateIndex(
                name: "IX_applicationForms_PreferenceId",
                table: "applicationForms",
                column: "PreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_applicationForms_TechnicalExperiencesId",
                table: "applicationForms",
                column: "TechnicalExperiencesId");

            migrationBuilder.CreateIndex(
                name: "IX_applicationForms_WorkExperiencesId",
                table: "applicationForms",
                column: "WorkExperiencesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicationForms");

            migrationBuilder.DropTable(
                name: "basicDetails");

            migrationBuilder.DropTable(
                name: "educations");

            migrationBuilder.DropTable(
                name: "experiences");

            migrationBuilder.DropTable(
                name: "LanguageDetails");

            migrationBuilder.DropTable(
                name: "Preference");

            migrationBuilder.DropTable(
                name: "techExpertises");
        }
    }
}
