using DMAWS_T2305M_Tran_Dinh_Lan.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Load the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext with connection string
builder.Services.AddDbContext<DMAWSContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container
builder.Services.AddControllers();

// Cấu hình Swagger cho API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DMAWS Project API",
        Version = "v1",
        Description = "API for managing Projects and Employees",
        Contact = new OpenApiContact
        {
            Name = "Tran Dinh Lan",
            Email = "tran.dinh.lan@example.com",
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DMAWS Project API v1");
        c.RoutePrefix = string.Empty; // Truy cập Swagger ở root URL (/) 
    });
}

// Sử dụng HTTPS trong ứng dụng
app.UseHttpsRedirection();

// Authorization middleware
app.UseAuthorization();

// Định tuyến các controller cho API
app.MapControllers();

// Chạy ứng dụng
app.Run();