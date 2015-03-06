public class Coal : IMachineType
{
	public string Name { get; set; }
	public int Cost { get; set; }
	public float Output { get; set; }
	public bool IsPoweredOn { get; private set; }
	public Range IncreasingTempRange { get; set; }
	public Range OptimalTempRange { get; set; }
	public float Temperature { get; set; }
	public bool IsOverloaded { get; set; }
	public float MaxOutputPerSecond { get; private set; }
	public float ShovelCost { get; set; }

	public Coal()
	{
		IncreasingTempRange = new Range(0, 0.7f);
		OptimalTempRange = new Range(IncreasingTempRange.High, 1f);
		IsPoweredOn = true;
		MaxOutputPerSecond = 30;
		ShovelCost = 1600;
	}

	public void TogglePower()
	{
		if(!IsOverloaded)
		{
			IsPoweredOn = !IsPoweredOn;
		}
	}
}