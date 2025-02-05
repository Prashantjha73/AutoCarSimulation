using AutoCarSimulation.ConsoleApp;
using AutoCarSimulation.ConsoleApp.Core.Interfaces;
using AutoCarSimulation.ConsoleApp.Core.Services;
using AutoCarSimulation.ConsoleApp.Infrastructure.Data;
using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;
internal class Program
{
    public static void Main(string[] args)
    {
        // Set up dependency injection.
        var services = new ServiceCollection();
        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        // Resolve and run the simulation runner.
        var runner = serviceProvider.GetRequiredService<SimulationRunner>();
        runner.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register abstractions with their concrete implementations.
        services.AddSingleton<ICarControlService, CarControlService>();
        services.AddSingleton<ICarStore, CarStore>();
        services.AddSingleton<IFieldStore, FieldStore>();
        services.AddTransient<ISimulationEngine, SimulationEngine>();
        services.AddSingleton<SimulationRunner>();
    }
}