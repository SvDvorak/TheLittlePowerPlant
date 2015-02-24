public struct Range
{
	public Range(float low, float high)
	{
		Low = low;
		High = high;
	}

	public float Low { get; private set; }
	public float High { get; private set; }
}