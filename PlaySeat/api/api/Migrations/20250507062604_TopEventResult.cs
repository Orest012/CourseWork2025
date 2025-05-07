using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class TopEventResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopEventResults",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventTitle = table.Column<string>(type: "text", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VenueName = table.Column<string>(type: "text", nullable: false),
                    TicketsSold = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopEventResults", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateOnly>(type: "date", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "venues",
                columns: table => new
                {
                    venue_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    capacity = table.Column<int>(type: "integer", nullable: true),
                    city = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    imageurl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("venues_pkey", x => x.venue_id);
                });

            migrationBuilder.CreateTable(
                name: "organizers",
                columns: table => new
                {
                    organizer_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    organization_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    contact_info = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("organizers_pkey", x => x.organizer_id);
                    table.ForeignKey(
                        name: "organizers_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "sport_events",
                columns: table => new
                {
                    sport_events_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    event_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    venue_id = table.Column<long>(type: "bigint", nullable: true),
                    organizer_id = table.Column<long>(type: "bigint", nullable: true),
                    sport_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sport_events_pkey", x => x.sport_events_id);
                    table.ForeignKey(
                        name: "sport_events_organizer_id_fkey",
                        column: x => x.organizer_id,
                        principalTable: "organizers",
                        principalColumn: "organizer_id");
                    table.ForeignKey(
                        name: "sport_events_venue_id_fkey",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "venue_id");
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    ticket_id = table.Column<long>(type: "bigint", nullable: false),
                    event_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    seat_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    section = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    is_sold = table.Column<bool>(type: "boolean", nullable: true),
                    purchased_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tickets_pkey", x => x.ticket_id);
                    table.ForeignKey(
                        name: "tickets_event_id_fkey",
                        column: x => x.event_id,
                        principalTable: "sport_events",
                        principalColumn: "sport_events_id");
                    table.ForeignKey(
                        name: "tickets_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    ticket_id = table.Column<long>(type: "bigint", nullable: true),
                    payment_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    payment_method = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("payments_pkey", x => x.payment_id);
                    table.ForeignKey(
                        name: "payments_ticket_id_fkey",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "ticket_id");
                    table.ForeignKey(
                        name: "payments_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_organizers_user_id",
                table: "organizers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_ticket_id",
                table: "payments",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_user_id",
                table: "payments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_sport_events_organizer_id",
                table: "sport_events",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "IX_sport_events_venue_id",
                table: "sport_events",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_event_id",
                table: "tickets",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_user_id",
                table: "tickets",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "TopEventResults");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "sport_events");

            migrationBuilder.DropTable(
                name: "organizers");

            migrationBuilder.DropTable(
                name: "venues");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
