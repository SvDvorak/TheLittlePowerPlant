using UnityEngine;

internal class UnityLogger : ILogger
{
	public void LogWarning(string message)
	{
		Debug.LogWarning(message);
	}
}