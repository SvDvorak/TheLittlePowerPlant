public class GameState
{
	public GameState()
	{
		Health = 1f;
	    LaserCharge = 1f;
		IsAlive = true;
	}

	public float Health { get; set; }
	public Range HealthRange { get { return new Range(0, Health); } }
	public bool IsAlive { get; set; }
	public float CurrentCityValue { get { return ScoreManager.CityValue - ScoreManager.DestructionValue; } }
	public float CrashTime { get; set; }
	public bool GameOver { get; set; }
	public bool GotHit { get; set; }
    public float LaserCharge { get; set; }
}