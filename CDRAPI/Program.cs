using CDRAPI.Data;
using CDRAPI.DTOs;
using CDRAPI.Interfaces;
using CDRAPI.Middleware;
using CDRAPI.Repository;
using CDRAPI.Services;
using CDRAPI.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Should the connection string be needed to for other services like Hangfire, logging
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUploadCallRecords, UploadCallRecordsService>();
builder.Services.AddTransient<IAppRepository, ApplicationRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services
    .AddHostedService<BackgroundWorker>()
    .AddSingleton<IBackgroundQueue<RequestInformation>, BackgroundQueue<RequestInformation>>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AverageCallValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//Adding this so you seed data and migration is run on first application run.
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();

    await Seed.SeedData(context);
}
catch(Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "There was a problem with migration or seed data population");
}

app.Run();
