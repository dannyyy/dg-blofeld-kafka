// See https://aka.ms/new-console-template for more information

using Chabis.Module.Deployment;
using Chabis.Module.Hosting;
using Chabis.Module.Hosting.Configuration;
using KafkaConsumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Kafka Consumer");
Console.WriteLine("===============");
Console.WriteLine();

var moduleConfiguration = ModuleConfiguration.CreateConfiguration(args);
var runConfiguration = moduleConfiguration.RunConfiguration;
var cancellationTokenSource = new CancellationTokenSource();

using var host = ModuleHost.CreateHost<ModuleStartup>(moduleConfiguration).Build();

try
{
    await host.RunAsync();
}
catch (Exception applicationException)
{
    cancellationTokenSource.Cancel(false);

    Console.WriteLine($"Application Runtime Exception: {applicationException.Message}");
    Console.WriteLine(applicationException.StackTrace);
}