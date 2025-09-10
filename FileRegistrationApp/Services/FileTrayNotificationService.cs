using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileRegistrationApp.Services;
public class FileTrayNotificationService : IDisposable, IFileAddedNotificator
{
	public event EventHandler<FileSystemEventArgs> FileAdded;
	public FileTrayNotificationService()
	{
		FileAdded += NotifyAboutFile;
	}
	public void OnFileAdded(FileSystemEventArgs eventArgs)
	{
		FileAdded?.Invoke(this, eventArgs);
	}
	public void Dispose()
	{
		FileAdded -= NotifyAboutFile;
	}
	private async void NotifyAboutFile(object sender, FileSystemEventArgs e)
	{
		NotifyIcon notifyIcon = new NotifyIcon
		{
			Icon = SystemIcons.Information,
			Visible = true,
			BalloonTipTitle = "File Registered!",
			BalloonTipText = $"Added \"{e.Name}\" at {DateTime.Now}"
		};
		int timeout = 3000;
		notifyIcon.ShowBalloonTip(timeout);
		await Task.Delay(timeout);
		notifyIcon.Dispose();
	}

}

public interface IFileAddedNotificator
{
	void OnFileAdded(FileSystemEventArgs eventArgs);
	event EventHandler<FileSystemEventArgs> FileAdded;
}
