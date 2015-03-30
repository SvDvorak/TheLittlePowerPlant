public class GameState
{
	public GameState()
	{
		Health = new Range(0, 1f);
		CityValue = 1234;
	}

	public Range Health { get; set; }
	public float CityValue { get; set; }
}