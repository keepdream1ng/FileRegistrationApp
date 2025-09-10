using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using NSubstitute;
using FileRegistrationApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileRegistrationApp.Tests;

public class IntegrationTest
{
	[Fact]
	public async Task FileWatcherService_TrowsOn_WrongPath()
	{
		string[] args = new string[] { "not a path" };
		var host = Program.CreateHostBuilder(args)
			.Build();

		await Assert.ThrowsAsync<PathNotFoundException>(() => host.StartAsync());
	}

	[Fact]
	public async Task FileWatcherService_NotifiesOn_FileAdded()
	{
		string[] args = new string[] { Path.GetTempPath() };
		var mockNotificator = Substitute.For<IFileAddedNotificator>();
		var host = Program.CreateHostBuilder(args)
			.ConfigureServices((_, services) =>
			{
                var originalNotificator = services.FirstOrDefault(d => d.ServiceType == typeof(IFileAddedNotificator));
                if (originalNotificator != null)
                {
                    services.Remove(originalNotificator);
                }
				services.AddSingleton<IFileAddedNotificator>(mockNotificator);
			})
			.Build();

		await host.StartAsync();
		string fullPath = Path.GetTempFileName();
		await host.StopAsync();

		mockNotificator
			.Received(1)
			.OnFileAdded(Arg.Is<FileSystemEventArgs>(x => x.FullPath == fullPath));
	}
}
