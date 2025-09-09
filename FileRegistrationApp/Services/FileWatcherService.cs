using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileRegistrationApp.Services;
public class FileWatcherService : IHostedService
{
	private FileSystemWatcher _watcher;
	private readonly FileWatherServiceConfig _config;
	private readonly ILogger<FileWatcherService> _logger;
	private readonly IFileAddedNotificator _fileAddedEventSource;

	public FileWatcherService(
		FileWatherServiceConfig config,
		IFileAddedNotificator fileAddedEventSource,
		ILogger<FileWatcherService>  logger
		)
    {
		_config = config;
		_logger = logger;
		_fileAddedEventSource = fileAddedEventSource;
	}

    public Task StartAsync(CancellationToken cancellationToken)
	{
		StartFileWatcher();
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		StopFileWatcher();
		return Task.CompletedTask;
	}

	private void StartFileWatcher()
	{
		_watcher = new FileSystemWatcher();
		_watcher.Path = _config.Path;
		_watcher.Created += NotifyAboutFile;
		_watcher.EnableRaisingEvents = true;
		_logger.LogInformation($"Wathing path: {_config.Path}");
	}

	private void StopFileWatcher()
	{
		if (_watcher is not null)
		{
			_watcher.Created -= NotifyAboutFile;
			_watcher.Dispose();
			_watcher = null;
			_logger.LogInformation("Watching stopped");
		}
	}

	private void NotifyAboutFile(object sender, FileSystemEventArgs e)
	{
		_logger.LogInformation($"New file added {e.Name}");
		_fileAddedEventSource.OnFileAdded(e);
	}
}

public class FileWatherServiceConfig
{
	public string Path { get; set; }
}
