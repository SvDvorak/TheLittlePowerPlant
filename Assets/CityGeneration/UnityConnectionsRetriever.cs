using UnityEngine;

public class UnityConnectionsRetriever : IExitRetriever
{
	public object GetExits(object tile, string name)
	{
		var gameObject = tile as GameObject;
		return gameObject.transform.FindChild(name);
	}
}