using ECFR.Data.EFModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add CORS services and define a policy

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ECFR.WebAPI.Configurations.CorsPolicy.ALLOW_ANY_ORIGINS,
        builder =>
        {
            builder.AllowAnyOrigin()
                    //.WithOrigins("http://localhost:4200", "http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


// Get the actual content root path
string contentRootPath = builder.Environment.ContentRootPath;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           .Replace("%CONTENTROOTPATH%", contentRootPath);

// Configure DbContext with the updated connection string
builder.Services.AddDbContext<EcfrdbMdfContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<ECFR.Services.ECFRDataFetcherService>();
builder.Services.AddScoped<ECFR.Services.AgencyService>();    
builder.Services.AddScoped<ECFR.Services.AgencyMetricsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(ECFR.WebAPI.Configurations.CorsPolicy.ALLOW_ANY_ORIGINS);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
