public class GameState
{
	public GameState()
	{
		Health = 1f;
		InitialCityValue = 1234;
		DestructionCost = 0;
		IsAlive = true;
	}

	public float Health { get; set; }
	public Range HealthRange { get { return new Range(0, Health); } }
	public bool IsAlive { get; set; }
	public float InitialCityValue { get; set; }
	public float CurrentCityValue { get { return InitialCityValue - DestructionCost; } }
	public float DestructionCost { get; set; }
	public float CrashTime { get; set; }
	public bool GameOver { get; set; }
	public bool GotHit { get; set; }
}