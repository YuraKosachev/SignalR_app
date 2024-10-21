
using Coravel;
using Microsoft.AspNetCore.SignalR;
using SignalR_app.Hubs;
using SignalR_app.Interfaces;
using SignalR_app.Jobs;
using SignalR_app.Services;

namespace SignalR_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddAuthorization();
            builder.Services.AddSignalR();
            builder.Services.AddScheduler();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IService, Service>();
            builder.Services.AddTransient<NotificationJob>();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapHub<ChatHub>("chat-hub");
            app.MapHub<NotificationHub>("notification");

            app.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<NotificationJob>()
                   .Cron("*/1 * * * *")
                   .Zoned(TimeZoneInfo.Local)
                   .PreventOverlapping(nameof(NotificationJob));
            });

            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) =>
            {
                await context.Clients.All.ReceiveMessage(message);
                return Results.NoContent();
            });

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            });

            app.MapGet("/forecast", async (string message, IServiceScopeFactory factory) =>
            {
                using var scope = factory.CreateScope();

                var service = scope.ServiceProvider.GetService<IService>();

                return await service.Do(message);

            })



            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.Run();
        }
    }
}
