using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;

namespace GHLearning.EasyDomainDrivenDesign.InfrastructureTests;

internal class WebApiApplicationTests(Action<IWebHostBuilder>? webHostConfigure = null) : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		_ = builder
			.ConfigureAppConfiguration((context, config) =>
			{
				// 清除原有的設定來源
				config.Sources.Clear();

				// 模擬載入 appsettings.json
				config
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
					.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
					.AddEnvironmentVariables();
			})
			.ConfigureServices(services => _ = services.AddSingleton<IConfigureOptions<OtlpExporterOptions>>(
				sp => new ConfigureNamedOptions<OtlpExporterOptions>(
					Options.DefaultName,
					exporterOptions => exporterOptions.TimeoutMilliseconds = 0)));

		webHostConfigure?.Invoke(builder);
	}
}
