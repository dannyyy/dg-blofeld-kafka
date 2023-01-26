using AutoMapper;
using Blofeld;
using Chabis.EventStreaming;
using Chabis.EventStreaming.Serialization;
using Chabis.Module.Hosting;
using Chabis.Module.Hosting.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KafkaConsumer;

public class ModuleStartup : ModuleStartupBase<ModuleStartup>
{
    public ModuleStartup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) : base(
        webHostEnvironment, configuration)
    {
    }

    protected override void ConfigureFeatures(IHostFeatureConfiguration features)
    {

    }

    protected override void ConfigureMvc(IMvcBuilder builder)
    {

    }

    protected override void ConfigureModuleServices(IServiceCollection services)
    {
        services.AddKafka(Configuration)
            .ConfigureMiddlewares(middleware => middleware.UseDefaultMiddlewares())
            .WithMultiProtocolSerialization(config => config.AddAvro())
            .AddKafkaConsumer<GameScoreUpdatedConsumer, int, GameScoreUpdated>();


        services.AddScoped<GameScoreUpdatedConsumer>();
    }

    protected override void ConfigureMappings(IMapperConfigurationExpression mapperConfig)
    {

    }

    protected override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env,
        IHostApplicationLifetime applicationLifetime)
    {

    }
}