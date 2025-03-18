using Pharmacy.Api.Extensions;
using Pharmacy.Api.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddInfrastructureServices();
builder.AddPersistenceServices();
builder.AddApplicationServices();
builder.AddApiServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(ApiConstant.CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapControllers();

app.Run();
