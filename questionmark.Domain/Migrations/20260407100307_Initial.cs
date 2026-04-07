using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace questionmark.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    session_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    session_hash = table.Column<byte[]>(type: "binary(32)", nullable: false),
                    session_nonce = table.Column<byte[]>(type: "binary(12)", nullable: false),
                    session_tag = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    session_end = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.session_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    auth = table.Column<byte[]>(type: "binary(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    post_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    text = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_of_post = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    user_id = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_post_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    comment_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    post_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    date_of_comment = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    text = table.Column<string>(type: "varchar(512)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_comment_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reaction",
                columns: table => new
                {
                    reaction_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    post_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    date_of_reaction = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    user_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    reaction = table.Column<ulong>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reaction", x => x.reaction_id);
                    table.ForeignKey(
                        name: "FK_reaction_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reaction_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_comment_post_id",
                table: "comment",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_user_id",
                table: "comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_user_id",
                table: "post",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reaction_user_id",
                table: "reaction",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uq_post_id_user_id",
                table: "reaction",
                columns: new[] { "post_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_session_session_hash",
                table: "session",
                column: "session_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_auth",
                table: "user",
                column: "auth",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "reaction");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
