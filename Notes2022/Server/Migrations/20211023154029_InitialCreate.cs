using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes2022.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimeZoneID = table.Column<int>(type: "int", nullable: false),
                    Ipref2 = table.Column<int>(type: "int", nullable: false),
                    Ipref3 = table.Column<int>(type: "int", nullable: false),
                    Ipref4 = table.Column<int>(type: "int", nullable: false),
                    Ipref5 = table.Column<int>(type: "int", nullable: false),
                    Ipref6 = table.Column<int>(type: "int", nullable: false),
                    Ipref7 = table.Column<int>(type: "int", nullable: false),
                    Ipref8 = table.Column<int>(type: "int", nullable: false),
                    Pref1 = table.Column<bool>(type: "bit", nullable: false),
                    Pref2 = table.Column<bool>(type: "bit", nullable: false),
                    Pref3 = table.Column<bool>(type: "bit", nullable: false),
                    Pref4 = table.Column<bool>(type: "bit", nullable: false),
                    Pref5 = table.Column<bool>(type: "bit", nullable: false),
                    Pref6 = table.Column<bool>(type: "bit", nullable: false),
                    Pref7 = table.Column<bool>(type: "bit", nullable: false),
                    Pref8 = table.Column<bool>(type: "bit", nullable: false),
                    MyStyle = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    MyGuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    AuditID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.AuditID);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCodes",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeviceCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "HomePageMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Posted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Use = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Algorithm = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsX509Certificate = table.Column<bool>(type: "bit", nullable: false),
                    DataProtected = table.Column<bool>(type: "bit", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkedFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeFileId = table.Column<int>(type: "int", nullable: false),
                    HomeFileName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RemoteFileName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RemoteBaseUri = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AcceptFrom = table.Column<bool>(type: "bit", nullable: false),
                    SendTo = table.Column<bool>(type: "bit", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkQueue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkedFileId = table.Column<int>(type: "int", nullable: false),
                    LinkGuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activity = table.Column<int>(type: "int", nullable: false),
                    BaseUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enqueued = table.Column<bool>(type: "bit", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberArchives = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NoteFileName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NoteFileTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastEdited = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "SQLFile",
                columns: table => new
                {
                    FileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contributor = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SQLFile", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "TZone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Offset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OffsetHours = table.Column<int>(type: "int", nullable: false),
                    OffsetMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TZone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mark",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NoteFileId = table.Column<int>(type: "int", nullable: false),
                    ArchiveId = table.Column<int>(type: "int", nullable: false),
                    MarkOrdinal = table.Column<int>(type: "int", nullable: false),
                    NoteOrdinal = table.Column<int>(type: "int", nullable: false),
                    NoteHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    ResponseOrdinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mark", x => new { x.UserId, x.NoteFileId, x.MarkOrdinal });
                    table.ForeignKey(
                        name: "FK_Mark_NoteFile_NoteFileId",
                        column: x => x.NoteFileId,
                        principalTable: "NoteFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteAccess",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NoteFileId = table.Column<int>(type: "int", nullable: false),
                    ArchiveId = table.Column<int>(type: "int", nullable: false),
                    ReadAccess = table.Column<bool>(type: "bit", nullable: false),
                    Respond = table.Column<bool>(type: "bit", nullable: false),
                    Write = table.Column<bool>(type: "bit", nullable: false),
                    SetTag = table.Column<bool>(type: "bit", nullable: false),
                    DeleteEdit = table.Column<bool>(type: "bit", nullable: false),
                    ViewAccess = table.Column<bool>(type: "bit", nullable: false),
                    EditAccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteAccess", x => new { x.UserID, x.NoteFileId, x.ArchiveId });
                    table.ForeignKey(
                        name: "FK_NoteAccess_NoteFile_NoteFileId",
                        column: x => x.NoteFileId,
                        principalTable: "NoteFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteHeader",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteFileId = table.Column<int>(type: "int", nullable: false),
                    ArchiveId = table.Column<int>(type: "int", nullable: false),
                    BaseNoteId = table.Column<long>(type: "bigint", nullable: false),
                    NoteOrdinal = table.Column<int>(type: "int", nullable: false),
                    ResponseOrdinal = table.Column<int>(type: "int", nullable: false),
                    NoteSubject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastEdited = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThreadLastEdited = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseCount = table.Column<int>(type: "int", nullable: false),
                    AuthorID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LinkGuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteHeader_NoteFile_NoteFileId",
                        column: x => x.NoteFileId,
                        principalTable: "NoteFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Search",
                columns: table => new
                {
                    NoteFileId = table.Column<int>(type: "int", nullable: false),
                    ArchiveId = table.Column<int>(type: "int", nullable: false),
                    BaseOrdinal = table.Column<int>(type: "int", nullable: false),
                    ResponseOrdinal = table.Column<int>(type: "int", nullable: false),
                    NoteID = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Option = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Search", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Search_NoteFile_NoteFileId",
                        column: x => x.NoteFileId,
                        principalTable: "NoteFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sequencer",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NoteFileId = table.Column<int>(type: "int", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    LastTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequencer", x => new { x.UserId, x.NoteFileId });
                    table.ForeignKey(
                        name: "FK_Sequencer_NoteFile_NoteFileId",
                        column: x => x.NoteFileId,
                        principalTable: "NoteFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteFileId = table.Column<int>(type: "int", nullable: false),
                    SubscriberId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_NoteFile_NoteFileId",
                        column: x => x.NoteFileId,
                        principalTable: "NoteFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SQLFileContent",
                columns: table => new
                {
                    SQLFileId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SQLFileContent", x => x.SQLFileId);
                    table.ForeignKey(
                        name: "FK_SQLFileContent_SQLFile_SQLFileId",
                        column: x => x.SQLFileId,
                        principalTable: "SQLFile",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteContent",
                columns: table => new
                {
                    NoteHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    NoteBody = table.Column<string>(type: "nvarchar(max)", maxLength: 100000, nullable: false),
                    DirectorMessage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteContent", x => x.NoteHeaderId);
                    table.ForeignKey(
                        name: "FK_NoteContent_NoteHeader_NoteHeaderId",
                        column: x => x.NoteHeaderId,
                        principalTable: "NoteHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    NoteHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NoteFileId = table.Column<int>(type: "int", nullable: false),
                    ArchiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => new { x.Tag, x.NoteHeaderId });
                    table.ForeignKey(
                        name: "FK_Tags_NoteHeader_NoteHeaderId",
                        column: x => x.NoteHeaderId,
                        principalTable: "NoteHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TZone",
                columns: new[] { "Id", "Abbreviation", "Name", "Offset", "OffsetHours", "OffsetMinutes" },
                values: new object[,]
                {
                    { 1, "GMT", "Greenwich Mean Time", "UTC+00", 0, 0 },
                    { 2, "WAKT", "Wake Island Time", "UTC+12", 12, 0 },
                    { 3, "CHAST", "Chatham Standard Time", "UTC+12:45", 12, 45 },
                    { 4, "NZDT", "New Zealand Daylight Time", "UTC+13", 13, 0 },
                    { 5, "PHOT", "Phoenix Island Time", "UTC+13", 13, 0 },
                    { 6, "TKT", "Tokelau Time", "UTC+13", 13, 0 },
                    { 7, "TOT", "Tonga Time", "UTC+13", 13, 0 },
                    { 8, "CHADT", "Chatham Daylight Time", "UTC+13:45", 13, 45 },
                    { 9, "LINT", "Line Islands Time", "UTC+14", 14, 0 },
                    { 10, "AZOST", "Azores Standard Time", "UTC-01", -1, 0 },
                    { 11, "CVT", "Cape Verde Time", "UTC-01", -1, 0 },
                    { 12, "EGT", "Eastern Greenland Time", "UTC-01", -1, 0 },
                    { 13, "BRST", "Brasilia Summer Time", "UTC-02", -2, 0 },
                    { 14, "FNT", "Fernando de Noronha Time", "UTC-02", -2, 0 },
                    { 15, "GST", "South Georgia and the South Sandwich Islands", "UTC-02", -2, 0 },
                    { 16, "PMDT", "Saint Pierre and Miquelon Daylight time", "UTC-02", -2, 0 },
                    { 17, "UYST", "Uruguay Summer Time", "UTC-02", -2, 0 },
                    { 18, "NDT", "Newfoundland Daylight Time", "UTC-02:30", -2, -30 },
                    { 19, "ADT", "Atlantic Daylight Time", "UTC-03", -3, 0 },
                    { 20, "AMST", "Amazon Summer Time (Brazil)[1]", "UTC-03", -3, 0 },
                    { 21, "ART", "Argentina Time", "UTC-03", -3, 0 },
                    { 22, "BRT", "Brasilia Time", "UTC-03", -3, 0 },
                    { 23, "TVT", "Tuvalu Time", "UTC+12", 12, 0 },
                    { 24, "PETT", "Kamchatka Time", "UTC+12", 12, 0 },
                    { 25, "NZST", "New Zealand Standard Time", "UTC+12", 12, 0 },
                    { 26, "MHT", "Marshall Islands", "UTC+12", 12, 0 },
                    { 27, "DDUT", "Dumont d'Urville Time", "UTC+10", 10, 0 },
                    { 28, "EST", "Eastern Standard Time (Australia)", "UTC+10", 10, 0 },
                    { 29, "PGT", "Papua New Guinea Time", "UTC+10", 10, 0 },
                    { 30, "VLAT", "Vladivostok Time", "UTC+10", 10, 0 },
                    { 31, "ACDT", "Australian Central Daylight Savings Time", "UTC+10:30", 10, 30 },
                    { 32, "CST", "Central Summer Time (Australia)", "UTC+10:30", 10, 30 },
                    { 33, "LHST", "Lord Howe Standard Time", "UTC+10:30", 10, 30 },
                    { 34, "AEDT", "Australian Eastern Daylight Savings Time", "UTC+11", 11, 0 },
                    { 35, "BST", "Bougainville Standard Time[4]", "UTC+11", 11, 0 },
                    { 36, "KOST", "Kosrae Time", "UTC+11", 11, 0 },
                    { 37, "CLST", "Chile Summer Time", "UTC-03", -3, 0 },
                    { 38, "LHST", "Lord Howe Summer Time", "UTC+11", 11, 0 },
                    { 39, "NCT", "New Caledonia Time", "UTC+11", 11, 0 },
                    { 40, "PONT", "Pohnpei Standard Time", "UTC+11", 11, 0 },
                    { 41, "SAKT", "Sakhalin Island time", "UTC+11", 11, 0 },
                    { 42, "SBT", "Solomon Islands Time", "UTC+11", 11, 0 }
                });

            migrationBuilder.InsertData(
                table: "TZone",
                columns: new[] { "Id", "Abbreviation", "Name", "Offset", "OffsetHours", "OffsetMinutes" },
                values: new object[,]
                {
                    { 43, "SRET", "Srednekolymsk Time", "UTC+11", 11, 0 },
                    { 44, "VUT", "Vanuatu Time", "UTC+11", 11, 0 },
                    { 45, "NFT", "Norfolk Time", "UTC+11:00", 11, 0 },
                    { 46, "FJT", "Fiji Time", "UTC+12", 12, 0 },
                    { 47, "GILT", "Gilbert Island Time", "UTC+12", 12, 0 },
                    { 48, "MAGT", "Magadan Time", "UTC+12", 12, 0 },
                    { 49, "MIST", "Macquarie Island Station Time", "UTC+11", 11, 0 },
                    { 50, "CHUT", "Chuuk Time", "UTC+10", 10, 0 },
                    { 51, "FKST", "Falkland Islands Standard Time", "UTC-03", -3, 0 },
                    { 52, "GFT", "French Guiana Time", "UTC-03", -3, 0 },
                    { 53, "PET", "Peru Time", "UTC-05", -5, 0 },
                    { 54, "CST", "Central Standard Time (North America)", "UTC-06", -6, 0 },
                    { 55, "EAST", "Easter Island Standard Time", "UTC-06", -6, 0 },
                    { 56, "GALT", "Galapagos Time", "UTC-06", -6, 0 },
                    { 57, "MDT", "Mountain Daylight Time (North America)", "UTC-06", -6, 0 },
                    { 58, "MST", "Mountain Standard Time (North America)", "UTC-07", -7, 0 },
                    { 59, "PDT", "Pacific Daylight Time (North America)", "UTC-07", -7, 0 },
                    { 60, "AKDT", "Alaska Daylight Time", "UTC-08", -8, 0 },
                    { 61, "CIST", "Clipperton Island Standard Time", "UTC-08", -8, 0 },
                    { 62, "PST", "Pacific Standard Time (North America)", "UTC-08", -8, 0 },
                    { 63, "AKST", "Alaska Standard Time", "UTC-09", -9, 0 },
                    { 64, "GAMT", "Gambier Islands", "UTC-09", -9, 0 },
                    { 65, "GIT", "Gambier Island Time", "UTC-09", -9, 0 },
                    { 66, "HADT", "Hawaii-Aleutian Daylight Time", "UTC-09", -9, 0 },
                    { 67, "MART", "Marquesas Islands Time", "UTC-09:30", -9, -30 },
                    { 68, "MIT", "Marquesas Islands Time", "UTC-09:30", -9, -30 },
                    { 69, "CKT", "Cook Island Time", "UTC-10", -10, 0 },
                    { 70, "HAST", "Hawaii-Aleutian Standard Time", "UTC-10", -10, 0 },
                    { 71, "HST", "Hawaii Standard Time", "UTC-10", -10, 0 },
                    { 72, "TAHT", "Tahiti Time", "UTC-10", -10, 0 },
                    { 73, "NUT", "Niue Time", "UTC-11", -11, 0 },
                    { 74, "EST", "Eastern Standard Time (North America)", "UTC-05", -5, 0 },
                    { 75, "ECT", "Ecuador Time", "UTC-05", -5, 0 },
                    { 76, "EASST", "Easter Island Standard Summer Time", "UTC-05", -5, 0 },
                    { 77, "CST", "Cuba Standard Time", "UTC-05", -5, 0 },
                    { 78, "PMST", "Saint Pierre and Miquelon Standard Time", "UTC-03", -3, 0 },
                    { 79, "PYST", "Paraguay Summer Time (South America)[8]", "UTC-03", -3, 0 },
                    { 80, "ROTT", "Rothera Research Station Time", "UTC-03", -3, 0 },
                    { 81, "SRT", "Suriname Time", "UTC-03", -3, 0 },
                    { 82, "UYT", "Uruguay Standard Time", "UTC-03", -3, 0 },
                    { 83, "NST", "Newfoundland Standard Time", "UTC-03:30", -3, -30 },
                    { 84, "NT", "Newfoundland Time", "UTC-03:30", -3, -30 }
                });

            migrationBuilder.InsertData(
                table: "TZone",
                columns: new[] { "Id", "Abbreviation", "Name", "Offset", "OffsetHours", "OffsetMinutes" },
                values: new object[,]
                {
                    { 85, "AMT", "Amazon Time (Brazil)[2]", "UTC-04", -4, 0 },
                    { 86, "AST", "Atlantic Standard Time", "UTC-04", -4, 0 },
                    { 87, "BOT", "Bolivia Time", "UTC-04", -4, 0 },
                    { 88, "FKST", "Falkland Islands Summer Time", "UTC-03", -3, 0 },
                    { 89, "CDT", "Cuba Daylight Time[5]", "UTC-04", -4, 0 },
                    { 90, "COST", "Colombia Summer Time", "UTC-04", -4, 0 },
                    { 91, "ECT", "Eastern Caribbean Time (does not recognise DST)", "UTC-04", -4, 0 },
                    { 92, "EDT", "Eastern Daylight Time (North America)", "UTC-04", -4, 0 },
                    { 93, "FKT", "Falkland Islands Time", "UTC-04", -4, 0 },
                    { 94, "GYT", "Guyana Time", "UTC-04", -4, 0 },
                    { 95, "PYT", "Paraguay Time (South America)[9]", "UTC-04", -4, 0 },
                    { 96, "VET", "Venezuelan Standard Time", "UTC-04:30", -4, -30 },
                    { 97, "ACT", "Acre Time", "UTC-05", -5, 0 },
                    { 98, "CDT", "Central Daylight Time (North America)", "UTC-05", -5, 0 },
                    { 99, "COT", "Colombia Time", "UTC-05", -5, 0 },
                    { 100, "CLT", "Chile Standard Time", "UTC-04", -4, 0 },
                    { 101, "ChST", "Chamorro Standard Time", "UTC+10", 10, 0 },
                    { 102, "AEST", "Australian Eastern Standard Time", "UTC+10", 10, 0 },
                    { 103, "CST", "Central Standard Time (Australia)", "UTC+09:30", 9, 30 },
                    { 104, "EEDT", "Eastern European Daylight Time", "UTC+03", 3, 0 },
                    { 105, "EEST", "Eastern European Summer Time", "UTC+03", 3, 0 },
                    { 106, "FET", "Further-eastern European Time", "UTC+03", 3, 0 },
                    { 107, "IDT", "Israel Daylight Time", "UTC+03", 3, 0 },
                    { 108, "IOT", "Indian Ocean Time", "UTC+03", 3, 0 },
                    { 109, "MSK", "Moscow Time", "UTC+03", 3, 0 },
                    { 110, "SYOT", "Showa Station Time", "UTC+03", 3, 0 },
                    { 111, "IRST", "Iran Standard Time", "UTC+03:30", 3, 30 },
                    { 112, "AMT", "Armenia Time", "UTC+04", 4, 0 },
                    { 113, "AZT", "Azerbaijan Time", "UTC+04", 4, 0 },
                    { 114, "GET", "Georgia Standard Time", "UTC+04", 4, 0 },
                    { 115, "GST", "Gulf Standard Time", "UTC+04", 4, 0 },
                    { 116, "MUT", "Mauritius Time", "UTC+04", 4, 0 },
                    { 117, "RET", "R?union Time", "UTC+04", 4, 0 },
                    { 118, "SAMT", "Samara Time", "UTC+04", 4, 0 },
                    { 119, "SCT", "Seychelles Time", "UTC+04", 4, 0 },
                    { 120, "VOLT", "Volgograd Time", "UTC+04", 4, 0 },
                    { 121, "AFT", "Afghanistan Time", "UTC+04:30", 4, 30 },
                    { 122, "IRDT", "Iran Daylight Time", "UTC+04:30", 4, 30 },
                    { 123, "HMT", "Heard and McDonald Islands Time", "UTC+05", 5, 0 },
                    { 124, "MAWT", "Mawson Station Time", "UTC+05", 5, 0 },
                    { 125, "EAT", "East Africa Time", "UTC+03", 3, 0 },
                    { 126, "AST", "Arabia Standard Time", "UTC+03", 3, 0 }
                });

            migrationBuilder.InsertData(
                table: "TZone",
                columns: new[] { "Id", "Abbreviation", "Name", "Offset", "OffsetHours", "OffsetMinutes" },
                values: new object[,]
                {
                    { 127, "WAST", "West Africa Summer Time", "UTC+02", 2, 0 },
                    { 128, "USZ1", "Kaliningrad Time", "UTC+02", 2, 0 },
                    { 129, "IBST", "International Business Standard Time", "UTC+00", 0, 0 },
                    { 130, "UCT", "Coordinated Universal Time", "UTC+00", 0, 0 },
                    { 131, "UTC", "Coordinated Universal Time", "UTC+00", 0, 0 },
                    { 132, "WET", "Western European Time", "UTC+00", 0, 0 },
                    { 133, "Z", "Zulu Time (Coordinated Universal Time)", "UTC+00", 0, 0 },
                    { 134, "EGST", "Eastern Greenland Summer Time", "UTC+00", 0, 0 },
                    { 135, "BST", "British Summer Time (British Standard Time from Feb 1968 to Oct 1971)", "UTC+01", 1, 0 },
                    { 136, "CET", "Central European Time", "UTC+01", 1, 0 },
                    { 137, "DFT", "AIX specific equivalent of Central European Time[6]", "UTC+01", 1, 0 },
                    { 138, "IST", "Irish Standard Time[7]", "UTC+01", 1, 0 },
                    { 139, "MVT", "Maldives Time", "UTC+05", 5, 0 },
                    { 140, "MET", "Middle European Time Same zone as CET", "UTC+01", 1, 0 },
                    { 141, "WEDT", "Western European Daylight Time", "UTC+01", 1, 0 },
                    { 142, "WEST", "Western European Summer Time", "UTC+01", 1, 0 },
                    { 143, "CAT", "Central Africa Time", "UTC+02", 2, 0 },
                    { 144, "CEDT", "Central European Daylight Time", "UTC+02", 2, 0 },
                    { 145, "CEST", "Central European Summer Time (Cf. HAEC)", "UTC+02", 2, 0 },
                    { 146, "EET", "Eastern European Time", "UTC+02", 2, 0 },
                    { 147, "HAEC", "Heure Avanc?e d'Europe Centrale francised name for CEST", "UTC+02", 2, 0 },
                    { 148, "IST", "Israel Standard Time", "UTC+02", 2, 0 },
                    { 149, "MEST", "Middle European Summer Time Same zone as CEST", "UTC+02", 2, 0 },
                    { 150, "SAST", "South African Standard Time", "UTC+02", 2, 0 },
                    { 151, "WAT", "West Africa Time", "UTC+01", 1, 0 },
                    { 152, "ORAT", "Oral Time", "UTC+05", 5, 0 },
                    { 153, "PKT", "Pakistan Standard Time", "UTC+05", 5, 0 },
                    { 154, "TFT", "Indian/Kerguelen", "UTC+05", 5, 0 },
                    { 155, "BDT", "Brunei Time", "UTC+08", 8, 0 },
                    { 156, "CHOT", "Choibalsan", "UTC+08", 8, 0 },
                    { 157, "CIT", "Central Indonesia Time", "UTC+08", 8, 0 },
                    { 158, "CST", "China Standard Time", "UTC+08", 8, 0 },
                    { 159, "CT", "China time", "UTC+08", 8, 0 },
                    { 160, "HKT", "Hong Kong Time", "UTC+08", 8, 0 },
                    { 161, "IRKT", "Irkutsk Time", "UTC+08", 8, 0 },
                    { 162, "MST", "Malaysia Standard Time", "UTC+08", 8, 0 },
                    { 163, "MYT", "Malaysia Time", "UTC+08", 8, 0 },
                    { 164, "PST", "Philippine Standard Time", "UTC+08", 8, 0 },
                    { 165, "AWST", "Australian Western Standard Time", "UTC+08", 8, 0 },
                    { 166, "SGT", "Singapore Time", "UTC+08", 8, 0 },
                    { 167, "ULAT", "Ulaanbaatar Time", "UTC+08", 8, 0 },
                    { 168, "WST", "Western Standard Time", "UTC+08", 8, 0 }
                });

            migrationBuilder.InsertData(
                table: "TZone",
                columns: new[] { "Id", "Abbreviation", "Name", "Offset", "OffsetHours", "OffsetMinutes" },
                values: new object[,]
                {
                    { 169, "CWST", "Central Western Standard Time (Australia) unofficial", "UTC+08:45", 8, 45 },
                    { 170, "AWDT", "Australian Western Daylight Time", "UTC+09", 9, 0 },
                    { 171, "EIT", "Eastern Indonesian Time", "UTC+09", 9, 0 },
                    { 172, "JST", "Japan Standard Time", "UTC+09", 9, 0 },
                    { 173, "KST", "Korea Standard Time", "UTC+09", 9, 0 },
                    { 174, "TLT", "Timor Leste Time", "UTC+09", 9, 0 },
                    { 175, "YAKT", "Yakutsk Time", "UTC+09", 9, 0 },
                    { 176, "ACST", "Australian Central Standard Time", "UTC+09:30", 9, 30 },
                    { 177, "SST", "Singapore Standard Time", "UTC+08", 8, 0 },
                    { 178, "SST", "Samoa Standard Time", "UTC-11", -11, 0 },
                    { 179, "ACT", "ASEAN Common Time", "UTC+08", 8, 0 },
                    { 180, "THA", "Thailand Standard Time", "UTC+07", 7, 0 },
                    { 181, "TJT", "Tajikistan Time", "UTC+05", 5, 0 },
                    { 182, "TMT", "Turkmenistan Time", "UTC+05", 5, 0 },
                    { 183, "UZT", "Uzbekistan Time", "UTC+05", 5, 0 },
                    { 184, "YEKT", "Yekaterinburg Time", "UTC+05", 5, 0 },
                    { 185, "IST", "Indian Standard Time", "UTC+05:30", 5, 30 },
                    { 186, "SLST", "Sri Lanka Standard Time", "UTC+05:30", 5, 30 },
                    { 187, "NPT", "Nepal Time", "UTC+05:45", 5, 45 },
                    { 188, "BDT", "Bangladesh Daylight Time (Bangladesh Daylight saving time keeps UTC+06 offset) [3]", "UTC+06", 6, 0 },
                    { 189, "BIOT", "British Indian Ocean Time", "UTC+06", 6, 0 },
                    { 190, "BST", "Bangladesh Standard Time", "UTC+06", 6, 0 },
                    { 191, "WIT", "Western Indonesian Time", "UTC+07", 7, 0 },
                    { 192, "BTT", "Bhutan Time", "UTC+06", 6, 0 },
                    { 193, "OMST", "Omsk Time", "UTC+06", 6, 0 },
                    { 194, "VOST", "Vostok Station Time", "UTC+06", 6, 0 },
                    { 195, "CCT", "Cocos Islands Time", "UTC+06:30", 6, 30 },
                    { 196, "MMT", "Myanmar Time", "UTC+06:30", 6, 30 },
                    { 197, "MST", "Myanmar Standard Time", "UTC+06:30", 6, 30 },
                    { 198, "CXT", "Christmas Island Time", "UTC+07", 7, 0 },
                    { 199, "DAVT", "Davis Time", "UTC+07", 7, 0 },
                    { 200, "HOVT", "Khovd Time", "UTC+07", 7, 0 },
                    { 201, "ICT", "Indochina Time", "UTC+07", 7, 0 },
                    { 202, "KRAT", "Krasnoyarsk Time", "UTC+07", 7, 0 },
                    { 203, "KGT", "Kyrgyzstan time", "UTC+06", 6, 0 },
                    { 204, "BIT", "Baker Island Time", "UTC-12", -12, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_DeviceCode",
                table: "DeviceCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_Expiration",
                table: "DeviceCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_Use",
                table: "Keys",
                column: "Use");

            migrationBuilder.CreateIndex(
                name: "IX_Mark_NoteFileId",
                table: "Mark",
                column: "NoteFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Mark_UserId_NoteFileId",
                table: "Mark",
                columns: new[] { "UserId", "NoteFileId" });

            migrationBuilder.CreateIndex(
                name: "IX_NoteAccess_NoteFileId",
                table: "NoteAccess",
                column: "NoteFileId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteHeader_LinkGuid",
                table: "NoteHeader",
                column: "LinkGuid");

            migrationBuilder.CreateIndex(
                name: "IX_NoteHeader_NoteFileId",
                table: "NoteHeader",
                column: "NoteFileId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteHeader_NoteFileId_ArchiveId",
                table: "NoteHeader",
                columns: new[] { "NoteFileId", "ArchiveId" });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_ConsumedTime",
                table: "PersistedGrants",
                column: "ConsumedTime");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Expiration",
                table: "PersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_SessionId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "SessionId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Search_NoteFileId",
                table: "Search",
                column: "NoteFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sequencer_NoteFileId",
                table: "Sequencer",
                column: "NoteFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_NoteFileId",
                table: "Subscription",
                column: "NoteFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NoteFileId",
                table: "Tags",
                column: "NoteFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NoteFileId_ArchiveId",
                table: "Tags",
                columns: new[] { "NoteFileId", "ArchiveId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NoteHeaderId",
                table: "Tags",
                column: "NoteHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "DeviceCodes");

            migrationBuilder.DropTable(
                name: "HomePageMessage");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "LinkedFile");

            migrationBuilder.DropTable(
                name: "LinkLog");

            migrationBuilder.DropTable(
                name: "LinkQueue");

            migrationBuilder.DropTable(
                name: "Mark");

            migrationBuilder.DropTable(
                name: "NoteAccess");

            migrationBuilder.DropTable(
                name: "NoteContent");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "Search");

            migrationBuilder.DropTable(
                name: "Sequencer");

            migrationBuilder.DropTable(
                name: "SQLFileContent");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TZone");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SQLFile");

            migrationBuilder.DropTable(
                name: "NoteHeader");

            migrationBuilder.DropTable(
                name: "NoteFile");
        }
    }
}
