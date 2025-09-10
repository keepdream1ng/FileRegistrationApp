# FileRegistrationApp

A .NET Framework 4.8 console application for monitoring and logging new file events in a specified folder.

## Features
- Watches a configured folder for newly created or copied files.
- Logs detected file creation events to both the console and a log file.
- Displays desktop tray notifications when new files are added (ensure system notifications are enabled to see them).
- Includes integration tests using `HostBuilder` with mockable dependencies.

## Configuration
You can configure the folder path in one of two ways:
1. **App.config** – set the default folder path in the configuration file.
2. **Command-line arguments** – pass the folder path as an argument when running the app.

## Usage
Example of running the application from the solution directory:

```bash
.\FileRegistrationApp\bin\Release\FileRegistrationApp.exe "C:\testing"
```
