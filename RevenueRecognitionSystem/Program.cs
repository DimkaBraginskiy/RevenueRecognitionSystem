

using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem;
using RevenueRecognitionSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



//adding scoped
builder.Services.AddControllers();
//builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);



var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseAuthorization();

app.MapControllers();

app.Run();