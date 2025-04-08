using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QLNhanSu.MyModels;
using System.Security.Claims;
using System.Text;
using QLNhanSu.Services;
using QLNhanSu.Settings;
using static QLNhanSu.Controllers.ChamCongQLController;

//var builder = WebApplication.CreateBuilder(args);

//// Lấy chuỗi kết nối từ appsettings.json
//string? strcnn = builder.Configuration.GetConnectionString("cnn");
//if (string.IsNullOrEmpty(strcnn))
//{
//    throw new InvalidOperationException("Không tìm thấy chuỗi kết nối trong cấu hình.");
//}

////builder.Services.AddSingleton<AttendanceService>();

//// Cấu hình DbContext
//builder.Services.AddDbContext<KetNoiCSDL>(options => options.UseSqlServer(strcnn));

//var jwtSettings = builder.Configuration.GetSection("Jwt");
//string? jwtKey = jwtSettings["Key"];
//if (string.IsNullOrEmpty(jwtKey))
//{
//    throw new Exception("❌ JWT Key is missing in appsettings.json!");
//}

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
//        if (builder.Environment.IsDevelopment())
//        {
//            Console.WriteLine("⚠️ Cảnh báo: RequireHttpsMetadata = false. Không nên dùng trong production!");
//        }

//        options.SaveToken = true;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Issuer"],
//            ValidAudience = jwtSettings["Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
//            RoleClaimType = ClaimTypes.Role
//        };
//    });

//// Cấu hình Authorization với Role-based Policy
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("QuanTriVien", policy => policy.RequireRole("QTV"));
//    options.AddPolicy("QuanLy", policy => policy.RequireRole("QL"));
//    options.AddPolicy("NhanVien", policy => policy.RequireRole("NV"));
//});

//builder.Services.AddHttpClient();

//// Thêm controllers
//builder.Services.AddControllers();

//// Cấu hình Swagger với JWT
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "QLNhanSu API",
//        Version = "v1",
//        Description = "API quản lý nhân sự sử dụng ASP.NET Core với JWT Authentication"
//    });

//    // ✅ Cấu hình nút "Authorize" để nhập JWT
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Nhập token theo định dạng: {your_token}"
//    });

//    // ✅ Sửa lỗi Swagger thêm "Bearer " hai lần
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
//                Scheme = "oauth2",
//                Name = "Bearer",
//                In = ParameterLocation.Header
//            },
//            new string[] {}
//        }
//    });
//});

//builder.Services.AddScoped<IAttendanceService, AttendanceService>();
//builder.Services.AddScoped<AttendanceSyncService>();


//builder.Services.AddHostedService<AttendanceSyncService>(); // Đảm bảo AttendanceSyncService là hosted service


//var app = builder.Build();

//// Bật CORS để tránh lỗi từ frontend
//app.UseCors(policy => policy
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//);
//// **QUAN TRỌNG: Bật Swagger UI LUÔN LUÔN**
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLNhanSu API v1");
//    c.RoutePrefix = "swagger"; // Giữ đường dẫn mặc định
//});

//// Bật xác thực và phân quyền
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối từ appsettings.json
string? strcnn = builder.Configuration.GetConnectionString("cnn");
if (string.IsNullOrEmpty(strcnn))
{
    throw new InvalidOperationException("Không tìm thấy chuỗi kết nối trong cấu hình.");
}

// Cấu hình DbContext
builder.Services.AddDbContext<KetNoiCSDL>(options => options.UseSqlServer(strcnn));

var jwtSettings = builder.Configuration.GetSection("Jwt");
string? jwtKey = jwtSettings["Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("❌ JWT Key is missing in appsettings.json!");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine("⚠️ Cảnh báo: RequireHttpsMetadata = false. Không nên dùng trong production!");
        }

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
            RoleClaimType = ClaimTypes.Role
        };
    });

// Cấu hình Authorization với Role-based Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("QuanTriVien", policy => policy.RequireRole("QTV"));
    options.AddPolicy("QuanLy", policy => policy.RequireRole("QL"));
    options.AddPolicy("NhanVien", policy => policy.RequireRole("NV"));
});

builder.Services.AddHttpClient();

// Thêm controllers
builder.Services.AddControllers();

// Cấu hình Swagger với JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QLNhanSu API",
        Version = "v1",
        Description = "API quản lý nhân sự sử dụng ASP.NET Core với JWT Authentication"
    });

    // ✅ Cấu hình nút "Authorize" để nhập JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo định dạng: {your_token}"
    });

    // ✅ Sửa lỗi Swagger thêm "Bearer " hai lần
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new string[] {}
        }
    });
});
// Thêm vào Program.cs

builder.Services.AddScoped<IAttendanceService, AttendanceService>(); // Đăng ký AttendanceService với Scoped
builder.Services.AddSingleton<AttendanceSyncService>(); // Đăng ký AttendanceSyncService với Singleton
builder.Services.AddHostedService(provider => provider.GetRequiredService<AttendanceSyncService>()); // Đảm bảo AttendanceSyncService được chạy dưới dạng background service


var app = builder.Build();

// Bật CORS để tránh lỗi từ frontend
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// **QUAN TRỌNG: Bật Swagger UI LUÔN LUÔN**
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLNhanSu API v1");
    c.RoutePrefix = "swagger"; // Giữ đường dẫn mặc định
});

// Bật xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
