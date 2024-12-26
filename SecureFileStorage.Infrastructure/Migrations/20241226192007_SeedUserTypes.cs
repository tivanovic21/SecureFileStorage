using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureFileStorage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_File_FileId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_User_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAccess_User_UserId",
                table: "FileAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileAccess",
                table: "FileAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityLogs",
                table: "ActivityLogs");

            migrationBuilder.RenameTable(
                name: "ActivityLogs",
                newName: "ActivityLog");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLog",
                newName: "IX_ActivityLog_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityLogs_FileId",
                table: "ActivityLog",
                newName: "IX_ActivityLog_FileId");

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "FileAccess",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "FileAccess",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileAccess",
                table: "FileAccess",
                columns: new[] { "FileId", "UserEmail" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityLog",
                table: "ActivityLog",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_UserTypeId",
                table: "User",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAccess_UserId1",
                table: "FileAccess",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_File_FileId",
                table: "ActivityLog",
                column: "FileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_User_UserId",
                table: "ActivityLog",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAccess_User_UserId",
                table: "FileAccess",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAccess_User_UserId1",
                table: "FileAccess",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserType_UserTypeId",
                table: "User",
                column: "UserTypeId",
                principalTable: "UserType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_File_FileId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_User_UserId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAccess_User_UserId",
                table: "FileAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAccess_User_UserId1",
                table: "FileAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserType_UserTypeId",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropIndex(
                name: "IX_User_UserTypeId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileAccess",
                table: "FileAccess");

            migrationBuilder.DropIndex(
                name: "IX_FileAccess_UserId1",
                table: "FileAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityLog",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "FileAccess");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "FileAccess");

            migrationBuilder.RenameTable(
                name: "ActivityLog",
                newName: "ActivityLogs");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityLog_UserId",
                table: "ActivityLogs",
                newName: "IX_ActivityLogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityLog_FileId",
                table: "ActivityLogs",
                newName: "IX_ActivityLogs_FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileAccess",
                table: "FileAccess",
                columns: new[] { "FileId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityLogs",
                table: "ActivityLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_File_FileId",
                table: "ActivityLogs",
                column: "FileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_User_UserId",
                table: "ActivityLogs",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAccess_User_UserId",
                table: "FileAccess",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
