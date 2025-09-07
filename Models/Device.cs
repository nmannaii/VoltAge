namespace VoltAge.Models;

public class Device
{
	public string? NativePath { get; set; }
	public string? Vendor { get; set; }
	public string? Model { get; set; }
	public string? Serial { get; set; }
	public bool? PowerSupply { get; set; }
	public Battery? Battery { get; set; }
}

public class Battery
{
	public bool? Preset { get; set; }
	public bool? Rechargeable { get; set; }
	public string? State { get; set; }
	public string? WarningLevel { get; set; }
	public string? Energy { get; set; }
	public string? EnergyEmpty { get; set; }
	public string? EnergyFull { get; set; }
	public string? EnergyFullDesign { get; set; }
	public string? VoltageMinDesign { get; set; }
	public string? CapacityLevel { get; set; }
	public string? EnergyRate { get; set; }
	public string? Voltage { get; set; }
	public int? ChargeCycles { get; set; }
	public float? Percentage { get; set; }
	public float? Capacity { get; set; }
	public string? Technology { get; set; }
	public string? IconName { get; set; }
}
