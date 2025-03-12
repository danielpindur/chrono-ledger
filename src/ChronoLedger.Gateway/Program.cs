using System.Text.Json.Serialization;
using ChronoLedger.Gateway.Routing;
using ChronoLedger.Gateway.Setup;
using ChronoLedger.Gateway.Swagger;
using ChronoLedger.Setup;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

builder.Services.Add(new ChronoLedgerInstaller());
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
