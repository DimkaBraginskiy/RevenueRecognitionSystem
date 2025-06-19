

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



//adding scoped
builder.Services.AddControllers();
//builder.Services.AddScoped<IDbService, DbService>();


/*builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);*/



var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseAuthorization();

app.MapControllers();

app.Run();