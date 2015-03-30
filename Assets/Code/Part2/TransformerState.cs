public class TransformerState
{
	public TransformerState()
	{
		Health = new Range(0, 1f);
	}

	public Range Health { get; set; }
}