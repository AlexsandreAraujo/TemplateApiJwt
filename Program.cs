using Microsoft.EntityFrameworkCore;
using TemplateApiJwt.Infraestrutura.Repositories;
using TemplateApiJwt.Domain.Model.EmployeeAggregate;
using TemplateApiJwt.Application.Mapping;
using TemplateApiJwt.Infraestrutura;
using TemplateApiJwt.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ConnectionContext>
    (options => options.UseNpgsql(builder.Configuration["Configurations:ConnectionStrings:DefaultConnection"]));

builder.Services.AddAutoMapper(typeof(DomainToDTOMapping));

builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddAuthenticationJwt(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
