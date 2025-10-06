using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FridgeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "BusinessInfos",
                columns: table => new
                {
                    BusinessInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MissionStatement = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    ServicesDescription = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    CoreValues = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    YearFounded = table.Column<int>(type: "int", nullable: false),
                    BusinessType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BannerImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessInfos", x => x.BusinessInfoId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    MenuCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false),
                    QuotationID = table.Column<int>(type: "int", nullable: false),
                    FridgeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ShopType = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Carts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "CustomerNote",
                columns: table => new
                {
                    CustomerNoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNote", x => x.CustomerNoteId);
                    table.ForeignKey(
                        name: "FK_CustomerNote_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Fridge",
                columns: table => new
                {
                    FridgeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FridgeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FridgeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WarrantyExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAdded = table.Column<DateOnly>(type: "date", nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    FaultID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fridge", x => x.FridgeId);
                    table.ForeignKey(
                        name: "FK_Fridge_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Fridge_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_Fridge_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId");
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Faults",
                columns: table => new
                {
                    FaultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaultDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplianceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InitialAssessment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EstimatedRepairTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequiredParts = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TechnicianID = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FridgeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faults", x => x.FaultID);
                    table.ForeignKey(
                        name: "FK_Faults_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Faults_Fridge_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                });

            migrationBuilder.CreateTable(
                name: "FridgeAllocation",
                columns: table => new
                {
                    AllocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    FridgeId = table.Column<int>(type: "int", nullable: false),
                    AllocationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReturnDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuantityAllocated = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FridgeAllocation", x => x.AllocationID);
                    table.ForeignKey(
                        name: "FK_FridgeAllocation_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_FridgeAllocation_Fridge_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    InventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FridgeID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.InventoryID);
                    table.ForeignKey(
                        name: "FK_Inventory_Fridge_FridgeID",
                        column: x => x.FridgeID,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                    table.ForeignKey(
                        name: "FK_Inventory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequest",
                columns: table => new
                {
                    MaintenanceRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FridgeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequest", x => x.MaintenanceRequestId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequest_Fridge_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                });

            migrationBuilder.CreateTable(
                name: "ScrappedFridges",
                columns: table => new
                {
                    FridgeID = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ScrapDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrappedFridges", x => x.FridgeID);
                    table.ForeignKey(
                        name: "FK_ScrappedFridges_Fridge_FridgeID",
                        column: x => x.FridgeID,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "RepairSchedules",
                columns: table => new
                {
                    RepairID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaultID = table.Column<int>(type: "int", nullable: false),
                    FridgeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RepairType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RepairNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PartsUsed = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequiredParts = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RepairCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualRepairTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiagnosisDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepairStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepairEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TestingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepairDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Employee = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairSchedules", x => x.RepairID);
                    table.ForeignKey(
                        name: "FK_RepairSchedules_Employees_Employee",
                        column: x => x.Employee,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_Faults_FaultID",
                        column: x => x.FaultID,
                        principalTable: "Faults",
                        principalColumn: "FaultID");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_Fridge_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                columns: table => new
                {
                    PurchaseRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FridgeId = table.Column<int>(type: "int", nullable: false),
                    InventoryID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ItemFullNames = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AssignedToRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    IsViewed = table.Column<bool>(type: "bit", nullable: false),
                    ViewedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequests", x => x.PurchaseRequestID);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Fridge_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Inventory_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "Inventory",
                        principalColumn: "InventoryID");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceVisit",
                columns: table => new
                {
                    MaintenanceVisitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FridgeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    MaintenanceRequestId = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    VisitNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceVisit", x => x.MaintenanceVisitId);
                    table.ForeignKey(
                        name: "FK_MaintenanceVisit_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_MaintenanceVisit_Fridge_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                    table.ForeignKey(
                        name: "FK_MaintenanceVisit_MaintenanceRequest_MaintenanceRequestId",
                        column: x => x.MaintenanceRequestId,
                        principalTable: "MaintenanceRequest",
                        principalColumn: "MaintenanceRequestId");
                });

            migrationBuilder.CreateTable(
                name: "RequestsForQuotation",
                columns: table => new
                {
                    RFQID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFQNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PurchaseRequestID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestsForQuotation", x => x.RFQID);
                    table.ForeignKey(
                        name: "FK_RequestsForQuotation_PurchaseRequests_PurchaseRequestID",
                        column: x => x.PurchaseRequestID,
                        principalTable: "PurchaseRequests",
                        principalColumn: "PurchaseRequestID");
                });

            migrationBuilder.CreateTable(
                name: "ComponentUsed",
                columns: table => new
                {
                    ComponentUsedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaintenanceVisitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentUsed", x => x.ComponentUsedId);
                    table.ForeignKey(
                        name: "FK_ComponentUsed_MaintenanceVisit_MaintenanceVisitId",
                        column: x => x.MaintenanceVisitId,
                        principalTable: "MaintenanceVisit",
                        principalColumn: "MaintenanceVisitId");
                });

            migrationBuilder.CreateTable(
                name: "FaultReport",
                columns: table => new
                {
                    FaultReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FaultType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrgencyLevel = table.Column<int>(type: "int", nullable: false),
                    FaultDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FridgeId = table.Column<int>(type: "int", nullable: false),
                    StatusFilter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintenanceVisitId = table.Column<int>(type: "int", nullable: false),
                    FaultID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultReport", x => x.FaultReportId);
                    table.ForeignKey(
                        name: "FK_FaultReport_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_FaultReport_Faults_FaultID",
                        column: x => x.FaultID,
                        principalTable: "Faults",
                        principalColumn: "FaultID");
                    table.ForeignKey(
                        name: "FK_FaultReport_Fridge_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridge",
                        principalColumn: "FridgeId");
                    table.ForeignKey(
                        name: "FK_FaultReport_MaintenanceVisit_MaintenanceVisitId",
                        column: x => x.MaintenanceVisitId,
                        principalTable: "MaintenanceVisit",
                        principalColumn: "MaintenanceVisitId");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceChecklist",
                columns: table => new
                {
                    MaintenanceCheckListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemperatureStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CondenserCoilsCleaned = table.Column<bool>(type: "bit", nullable: false),
                    CoolantLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoorSealCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LightingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PowerCableCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MaintenanceVisitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceChecklist", x => x.MaintenanceCheckListId);
                    table.ForeignKey(
                        name: "FK_MaintenanceChecklist_MaintenanceVisit_MaintenanceVisitId",
                        column: x => x.MaintenanceVisitId,
                        principalTable: "MaintenanceVisit",
                        principalColumn: "MaintenanceVisitId");
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    QuotationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestForQuotationId = table.Column<int>(type: "int", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuotationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.QuotationID);
                    table.ForeignKey(
                        name: "FK_Quotations_RequestsForQuotation_RequestForQuotationId",
                        column: x => x.RequestForQuotationId,
                        principalTable: "RequestsForQuotation",
                        principalColumn: "RFQID");
                    table.ForeignKey(
                        name: "FK_Quotations_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuotationID = table.Column<int>(type: "int", nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.PurchaseOrderID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Quotations_QuotationID",
                        column: x => x.QuotationID,
                        principalTable: "Quotations",
                        principalColumn: "QuotationID");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryNotes",
                columns: table => new
                {
                    DeliveryNoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantityDelivered = table.Column<int>(type: "int", nullable: false),
                    ReceivedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsReceivedInInventory = table.Column<bool>(type: "bit", nullable: false),
                    InventoryReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryNotes", x => x.DeliveryNoteID);
                    table.ForeignKey(
                        name: "FK_DeliveryNotes_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderID");
                    table.ForeignKey(
                        name: "FK_DeliveryNotes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] { 1, "Fridges" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge1.jpg", "Bosch Fridge 1", 9586m },
                    { 2, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge2.jpg", "Bosch Fridge 2", 3807m },
                    { 3, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge3.jpg", "Whirlpool Fridge 3", 5394m },
                    { 4, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge4.jpg", "Bosch Fridge 4", 7106m },
                    { 5, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge5.jpg", "Defy Fridge 5", 6581m },
                    { 6, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge6.jpg", "Bosch Fridge 6", 9097m },
                    { 7, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge7.jpg", "Bosch Fridge 7", 8978m },
                    { 8, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge8.jpg", "LG Fridge 8", 11223m },
                    { 9, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge9.jpg", "Defy Fridge 9", 7911m },
                    { 10, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge10.jpg", "Whirlpool Fridge 10", 7690m },
                    { 11, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge11.jpg", "Bosch Fridge 11", 4320m },
                    { 12, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge12.jpg", "Samsung Fridge 12", 3564m },
                    { 13, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge13.jpg", "Hisense Fridge 13", 7988m },
                    { 14, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge14.jpg", "Defy Fridge 14", 10109m },
                    { 15, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge15.jpg", "LG Fridge 15", 6075m },
                    { 16, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge16.jpg", "Defy Fridge 16", 7436m },
                    { 17, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge17.jpg", "Whirlpool Fridge 17", 10751m },
                    { 18, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge18.jpg", "Samsung Fridge 18", 9659m },
                    { 19, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge19.jpg", "Samsung Fridge 19", 7089m },
                    { 20, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge20.jpg", "Samsung Fridge 20", 10770m },
                    { 21, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge21.jpg", "Hisense Fridge 21", 9375m },
                    { 22, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge22.jpg", "Whirlpool Fridge 22", 5580m },
                    { 23, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge23.jpg", "Hisense Fridge 23", 7893m },
                    { 24, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge24.jpg", "Bosch Fridge 24", 7028m },
                    { 25, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge25.jpg", "LG Fridge 25", 9117m },
                    { 26, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge26.jpg", "LG Fridge 26", 10871m },
                    { 27, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge27.jpg", "LG Fridge 27", 6112m },
                    { 28, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge28.jpg", "Defy Fridge 28", 11749m },
                    { 29, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge29.jpg", "Samsung Fridge 29", 9473m },
                    { 30, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge30.jpg", "Defy Fridge 30", 10000m }
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
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentUsed_MaintenanceVisitId",
                table: "ComponentUsed",
                column: "MaintenanceVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNote_CustomerId",
                table: "CustomerNote",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ApplicationUserId",
                table: "Customers",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LocationId",
                table: "Customers",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNotes_PurchaseOrderId",
                table: "DeliveryNotes",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNotes_SupplierId",
                table: "DeliveryNotes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LocationId",
                table: "Employees",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReport_CustomerID",
                table: "FaultReport",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReport_FaultID",
                table: "FaultReport",
                column: "FaultID");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReport_FridgeId",
                table: "FaultReport",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReport_MaintenanceVisitId",
                table: "FaultReport",
                column: "MaintenanceVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_EmployeeID",
                table: "Faults",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_FridgeId",
                table: "Faults",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fridge_CustomerId",
                table: "Fridge",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fridge_LocationId",
                table: "Fridge",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Fridge_SerialNumber",
                table: "Fridge",
                column: "SerialNumber",
                unique: true,
                filter: "[SerialNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Fridge_SupplierID",
                table: "Fridge",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeAllocation_CustomerID",
                table: "FridgeAllocation",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeAllocation_FridgeId",
                table: "FridgeAllocation",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_FridgeID",
                table: "Inventory",
                column: "FridgeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceChecklist_MaintenanceVisitId",
                table: "MaintenanceChecklist",
                column: "MaintenanceVisitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_FridgeId",
                table: "MaintenanceRequest",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceVisit_EmployeeID",
                table: "MaintenanceVisit",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceVisit_FridgeId",
                table: "MaintenanceVisit",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceVisit_MaintenanceRequestId",
                table: "MaintenanceVisit",
                column: "MaintenanceRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_QuotationID",
                table: "PurchaseOrders",
                column: "QuotationID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierID",
                table: "PurchaseOrders",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CustomerID",
                table: "PurchaseRequests",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_FridgeId",
                table: "PurchaseRequests",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_InventoryID",
                table: "PurchaseRequests",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_RequestForQuotationId",
                table: "Quotations",
                column: "RequestForQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_SupplierId",
                table: "Quotations",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_Employee",
                table: "RepairSchedules",
                column: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_FaultID",
                table: "RepairSchedules",
                column: "FaultID");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_FridgeId",
                table: "RepairSchedules",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestsForQuotation_PurchaseRequestID",
                table: "RequestsForQuotation",
                column: "PurchaseRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId",
                table: "Reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminNotifications");

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
                name: "BusinessInfos");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "ComponentUsed");

            migrationBuilder.DropTable(
                name: "CustomerNote");

            migrationBuilder.DropTable(
                name: "DeliveryNotes");

            migrationBuilder.DropTable(
                name: "FaultReport");

            migrationBuilder.DropTable(
                name: "FridgeAllocation");

            migrationBuilder.DropTable(
                name: "MaintenanceChecklist");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RepairSchedules");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ScrappedFridges");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "MaintenanceVisit");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Faults");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "MaintenanceRequest");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "RequestsForQuotation");

            migrationBuilder.DropTable(
                name: "PurchaseRequests");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Fridge");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
