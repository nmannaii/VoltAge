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

var devices = UPower.LoadDevicesInfos();

var table = new Table()
    .Border(TableBorder.Rounded)
    .BorderColor(Color.Grey)
    .ShowRowSeparators()
    .Title("[bold yellow] VoltAge — Power Devices[/]")
    .AddColumn(new TableColumn("[bold cyan]Native Path[/]").LeftAligned())
    .AddColumn(new TableColumn("[bold cyan]Vendor[/]").Centered())
    .AddColumn(new TableColumn("[bold cyan]Model[/]").Centered())
    .AddColumn(new TableColumn("[bold cyan]Power Supply[/]").Centered())
    .AddColumn(new TableColumn("[bold cyan]State[/]").Centered())
    .AddColumn(new TableColumn("[bold cyan]Percentage[/]").RightAligned())
    .AddColumn(new TableColumn("[bold cyan]Energy[/]").RightAligned())
    .AddColumn(new TableColumn("[bold cyan]Energy Full[/]").RightAligned())
    .AddColumn(new TableColumn("[bold cyan]Voltage[/]").RightAligned())
    .AddColumn(new TableColumn("[bold cyan]Capacity[/]").RightAligned())
    .AddColumn(new TableColumn("[bold cyan]Warning Level[/]").Centered());

foreach (var device in devices)
{
    string pct = device.Battery?.Percentage is float p
        ? p switch
        {
            >= 80 => $"[green]{p}%[/]",
            >= 40 => $"[yellow]{p}%[/]",
            _     => $"[red]{p}%[/]"
        }
        : "[grey]-[/]";

    string state = device.Battery?.State switch
    {
        "charging"    => "[green]charging[/]",
        "discharging" => "[yellow]discharging[/]",
        "fully-charged" => "[bold green]fully-charged[/]",
        null or ""    => "[grey]-[/]",
        var s         => s
    };

    table.AddRow(
        device.NativePath     ?? "[grey]-[/]",
        device.Vendor         ?? "[grey]-[/]",
        device.Model          ?? "[grey]-[/]",
        device.PowerSupply is true ? "[green]yes[/]" : "[red]no[/]",
        state,
        pct,
        device.Battery?.Energy          ?? "[grey]-[/]",
        device.Battery?.EnergyFull      ?? "[grey]-[/]",
        device.Battery?.Voltage         ?? "[grey]-[/]",
        device.Battery?.Capacity is float c ? $"{c}%" : "[grey]-[/]",
        device.Battery?.WarningLevel    ?? "[grey]-[/]"
    );
}

AnsiConsole.Write(table);

