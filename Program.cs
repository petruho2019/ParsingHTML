using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using ParsingHTML.Filters;
using ParsingHTML.Services.Abstractions;
using ParsingHTML.Services.Concrete;

namespace ParsingHTML;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;
        var conf = builder.Configuration;
        var connString = conf.GetConnectionString("DefaultConnection");

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<IValidator<ParseRequestModel>, ParseRequestModelValidator>();
        services.AddScoped<IHtmlElementsService, HtmlElementService>();
        services.AddScoped<IElementsRepository, ElementsRepository>(prv => new(connString!));

        services.AddControllers(opts =>
        {
            opts.Filters.Add<MainExceptionFilter>();
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = "api/swagger";
        });

        await DBInitializer.InitializeDatabase(connString!);

        app.MapControllers();

        app.Run();
    }
}