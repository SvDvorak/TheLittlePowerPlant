
public interface IRandom
{
	int Range(int min, int max);
	float Range(float min, float max);
}

public class Random : IRandom
{
	public int Range(int min, int max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	public float Range(float min, float max)
	{
		return UnityEngine.Random.Range(min, max);
	}
}