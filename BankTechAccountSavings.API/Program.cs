using BankTechAccountSavings.API.Config;
using BankTechAccountSavings.Application;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Infraestructure;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication()
    .AddRepositories()
    .AddFluentValidation();

//Add AutoMapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Configure connection string
builder.Services.ConfigDbConnection(builder.Configuration);

builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<AccountStatus>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetValues(typeof(AccountStatus)).Cast<AccountStatus>()
        .Select(value =>
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return new OpenApiString((attribute?.Name) ?? value.ToString());
        })
        .ToList<IOpenApiAny>()
    });
    c.MapType<Currency>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetValues(typeof(Currency)).Cast<Currency>()
        .Select(value =>
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return new OpenApiString((attribute?.Name) ?? value.ToString());
        })
        .ToList<IOpenApiAny>()
    });
    c.MapType<TransactionStatus>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>()
        .Select(value =>
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return new OpenApiString((attribute?.Name) ?? value.ToString());
        })
        .ToList<IOpenApiAny>()
    });
    c.MapType<TransferType>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetValues(typeof(TransferType)).Cast<TransferType>()
        .Select(value =>
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return new OpenApiString((attribute?.Name) ?? value.ToString());
        })
        .ToList<IOpenApiAny>()
    });
    c.MapType<TransactionType>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>()
       .Select(value =>
       {
           var fieldInfo = value.GetType().GetField(value.ToString());
           var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
           return new OpenApiString((attribute?.Name) ?? value.ToString());
       })
       .ToList<IOpenApiAny>()
    });

});

var app = builder.Build();

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
