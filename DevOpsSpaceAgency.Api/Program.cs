using DevOpsSpaceAgency.Api.Services;
using DevOpsSpaceAgency.BLL.Services;
using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.DAL.Contexts;
using DevOpsSpaceAgency.DAL.Repositories;
using DevOpsSpaceAgency.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SpaceAgencyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<ISpaceModuleService, SpaceModuleService>();

builder.Services.AddHttpClient<IIssAstronautProvider, OpenNotifyIssAstronautProvider>(client =>
{
    client.BaseAddress = new Uri("http://api.open-notify.org/");
});

builder.Services.AddHttpClient<IIssPositionProvider, OpenNotifyIssPositionProvider>(client =>
{
    client.BaseAddress = new Uri("http://api.open-notify.org/");
});

builder.Services.AddHttpClient<IApodProvider, NasaApodProvider>(client =>
{
    client.BaseAddress = new Uri("https://api.nasa.gov/");
});

builder.Services.AddHttpClient<IModuleStatusProvider, HttpStatModuleStatusProvider>(client =>
{
    client.BaseAddress = new Uri("https://httpstat.us/");
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    SpaceAgencyContext dbContext = scope.ServiceProvider.GetRequiredService<SpaceAgencyContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowReact");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
