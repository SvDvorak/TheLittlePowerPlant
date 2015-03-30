public class GameState
{
	public GameState()
	{
		Health = 1f;
		CityValue = 1234;
		IsAlive = true;
	}

	public float Health { get; set; }
	public Range HealthRange { get { return new Range(0, Health); } }
	public bool IsAlive { get; set; }
	public float CityValue { get; set; }
	public float CrashTime { get; set; }
	public bool GameOver { get; set; }
}