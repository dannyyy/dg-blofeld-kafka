// See https://aka.ms/new-console-template for more information

using Chabis.Module.Deployment;
using Chabis.Module.Hosting;
using Chabis.Module.Hosting.Configuration;
using KafkaProducer;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Kafka Publisher");
Console.WriteLine("===============");
Console.WriteLine();

var moduleConfiguration = ModuleConfiguration.CreateConfiguration(args);
var runConfiguration = moduleConfiguration.RunConfiguration;
var cancellationTokenSource = new CancellationTokenSource();

using var host = ModuleHost.CreateHost<ModuleStartup>(moduleConfiguration).Build();

try
{
    if (runConfiguration.IsDeploymentRun)
    {
        // IModuleDeployment deployments
        var deploymentConsole = host.Services.GetRequiredService<DeploymentConsole>();
        await deploymentConsole.RunDeploymentsAsync();

        return;
    }

    await host.StartAsync();

    var runner = host.Services.GetRequiredService<Runner>();
    await runner.PublishAsync(cancellationTokenSource.Token);

    await host.StopAsync(cancellationTokenSource.Token);

}
catch (Exception applicationException)
{
    cancellationTokenSource.Cancel(false);

    Console.WriteLine($"Application Runtime Exception: {applicationException.Message}");
    Console.WriteLine(applicationException.StackTrace);
}