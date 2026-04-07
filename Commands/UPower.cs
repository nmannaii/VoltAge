using System.Diagnostics;
using System.Text.RegularExpressions;
using VoltAge.Models;

namespace VoltAge.Commands;

public static class UPower
{
	public static bool IsCommandAvailable()
	{
		try
		{
			string output = RunCommand("which", ["upower"]);
			return !string.IsNullOrEmpty(output);
		}
		catch
		{
			return false;
		}
	}


	public static List<Device> LoadDevicesInfos()
	{
		// Get devices
		string[] devicesEnums = RunCommand("upower", ["-e"]).Split("\n");
		List<Device> devices = [];
		foreach (string deviceEnum in devicesEnums)
		{
			string deviceInfo = RunCommand("upower", ["-i", deviceEnum]);
			Device device = new()
			{
				Battery = new Battery()
			};
			foreach (string line in deviceInfo.Split("\n"))
			{
				var trimmed = line.Trim();
				if (string.IsNullOrEmpty(trimmed)) continue;

				if (trimmed.StartsWith("native-path:"))
					device.NativePath = trimmed.Split(':')[1].Trim();
				else if (trimmed.StartsWith("vendor:"))
					device.Vendor = trimmed.Split(':')[1].Trim();
				else if (trimmed.StartsWith("model:"))
					device.Model = trimmed.Split(':')[1].Trim();
				else if (trimmed.StartsWith("serial:"))
					device.Serial = trimmed.Split(':')[1].Trim();
				else if (trimmed.StartsWith("power supply:"))
					device.PowerSupply = trimmed.Split(':')[1].Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);

				// battery info
				else if (trimmed.StartsWith("present:"))
					device.Battery.Preset = trimmed.Split(':')[1].Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);
				else if (trimmed.StartsWith("rechageable:"))
					device.Battery.Rechargeable = trimmed.Split(':')[1].Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);
				else if (trimmed.StartsWith("state:"))
					device.Battery.State = trimmed.Split(':')[1].Trim();
				else if (trimmed.StartsWith("warning-level:"))
					device.Battery.WarningLevel = trimmed.Split(':')[1].Trim();
				else if (trimmed.StartsWith("energy:"))
					device.Battery.Energy = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("energy-empty:"))
					device.Battery.EnergyEmpty = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("energy-full:"))
					device.Battery.EnergyFull = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("energy-full-design:"))
					device.Battery.EnergyFullDesign = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("volate-min-design:"))
					device.Battery.VoltageMinDesign = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("capacity-level:"))
					device.Battery.CapacityLevel = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("energy-rate:"))
					device.Battery.EnergyRate = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("voltage:"))
					device.Battery.Voltage = trimmed.Split(":")[1].Trim();
				else if (trimmed.StartsWith("charge-cycles:"))
					device.Battery.ChargeCycles = Regex.Match(trimmed, @"\d+").Value == "N/A" ? int.Parse(Regex.Match(trimmed, @"\d+").Value) : 0;
				else if (trimmed.StartsWith("percentage:"))
					device.Battery.Percentage = float.Parse(Regex.Match(trimmed, @"\d+").Value);
				else if (trimmed.StartsWith("capacity:"))
					device.Battery.Capacity = float.Parse(Regex.Match(trimmed, @"\d+").Value);
			}
			devices.Add(device);
		}
		return devices;
	}

	private static string RunCommand(string command, string[] args)
	{
		Process process = new()
		{
			StartInfo = new()
			{
				FileName = command,
				Arguments = string.Join(" ", args),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			}
		};

		process.Start();
		process.WaitForExit();

		return process.StandardOutput.ReadToEnd().Trim();
	}
}
