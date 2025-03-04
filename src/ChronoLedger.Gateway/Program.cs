using System.Text.Json.Serialization;
using ChronoLedger.Gateway.Routing;
using ChronoLedger.Gateway.Setup;
using ChronoLedger.Gateway.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options =>
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new LowerCaseTransformer()));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SchemaFilter<EnumSchemaFilter>();
    });

GatewayInstaller.Install(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
