using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem;
using RevenueRecognitionSystem.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RevenueRecognitionSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var jwtConfig = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtConfig["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
        };
    });

builder.Services.AddAuthorization();



//adding scoped
builder.Services.AddControllers();
//builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddHttpClient<ICurrencyExchangeService, CurrencyExchangeService>();
builder.Services.AddScoped<IRevenueService, RevenueService>();

builder.Services.AddScoped<ClientsRepository>();
builder.Services.AddScoped<ContractRepository>();
builder.Services.AddScoped<DiscountRepository>();
builder.Services.AddScoped<SoftwareRepository>();
builder.Services.AddScoped<PaymentRepository>();





builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();