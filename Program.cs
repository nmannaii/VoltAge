using System.Reflection;
using System.Runtime.InteropServices;
using Spectre.Console;
using VoltAge.Commands;
using VoltAge.Enums;

string? APP_NAME = Assembly.GetExecutingAssembly().GetName().Name;

if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
	AnsiConsole.MarkupLine($"[red][bold]{ErrorTypes.FATAL}:[/] {APP_NAME} not supported for your platform {RuntimeInformation.OSDescription}[/]");
}

if (!UPower.IsCommandAvailable())
{
	AnsiConsole.MarkupLine(@$"[red][bold]{ErrorTypes.FATAL}:[/] upower command not found.[/]");
	AnsiConsole.MarkupLine("[link]Visit https://upower.freedesktop.org[/] for more information");
}

UPower.LoadDevicesInfos(@"\d+");

