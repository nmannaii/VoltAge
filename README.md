# VoltAge

A Linux CLI tool that queries power device information via `upower` and displays it in a rich, color-coded terminal table.

## Requirements

- Linux
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [`upower`](https://upower.freedesktop.org) installed and available in `$PATH`

  ```bash
  # Debian / Ubuntu
  sudo apt install upower

  # Arch
  sudo pacman -S upower

  # Fedora
  sudo dnf install upower
  ```

## Build & Run

```bash
# Clone
git clone <repo-url>
cd VoltAge

# Run directly
dotnet run

# Or build and execute the binary
dotnet build -c Release
./bin/Release/net9.0/VoltAge
```

## Output

VoltAge prints a rounded table with one row per power device, including:

| Column        | Description                                    |
|---------------|------------------------------------------------|
| Native Path   | Kernel device path                             |
| Vendor        | Hardware vendor                                |
| Model         | Device model name                              |
| Power Supply  | Whether the device is a power supply           |
| State         | Current charge state (charging / discharging)  |
| Percentage    | Remaining charge                               |
| Energy        | Current energy (Wh)                            |
| Energy Full   | Energy at full charge (Wh)                     |
| Voltage       | Current voltage (V)                            |
| Capacity      | Battery health capacity                        |
| Warning Level | Active warning level                           |

### Color coding

- **Percentage** — green ≥ 80%, yellow ≥ 40%, red < 40%
- **State** — green for `charging` / `fully-charged`, yellow for `discharging`
- **Power Supply** — green `yes`, red `no`

## Project Structure

```
VoltAge/
├── Program.cs           # Entry point — builds and renders the table
├── Commands/
│   └── UPower.cs        # Wraps upower CLI calls and parses output
├── Models/
│   └── Device.cs        # Device and Battery model classes
├── Enums/
│   └── ErrorTypes.cs    # Error type constants
└── VoltAge.csproj
```

## Dependencies

| Package               | Version |
|-----------------------|---------|
| Spectre.Console       | 0.50.0  |
| Spectre.Console.Cli   | 0.50.0  |
