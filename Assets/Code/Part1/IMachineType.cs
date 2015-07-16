public interface IMachineType
{
	string Name { get; set; }
	int Cost { get; set; }
	float Output { get; set; }
	bool IsPoweredOn { get; }
	bool IsOverloaded { get; set; }
	float MaxOutputPerSecond { get; }
}