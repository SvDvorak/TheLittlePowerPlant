using System;
using System.Collections.Generic;
using System.Linq;

public interface IConnectionsFinder
{
	List<string> FindConnections(Func<string, object> getExit);
}

public class ConnectionsFinder : IConnectionsFinder
{
	private readonly List<string> _directions;
	private Func<string, object> _getExit;

	public ConnectionsFinder()
	{
		_directions = new List<string> { "N", "E", "S", "W" };
	}

	public List<string> FindConnections(Func<string, object> getExit)
	{
		_getExit = getExit;
		return SubdivideAndCalculate().ToList();
	}

	private IEnumerable<string> SubdivideAndCalculate()
	{
		var connections = new List<string>();
		for (var subdivideCount = _directions.Count; subdivideCount > 0; subdivideCount--)
		{
			connections.AddRange(RotateAndCalculate(subdivideCount));
		}
		return connections;
	}

	private IEnumerable<string> RotateAndCalculate(int takeCount)
	{
		for (var i = 0; i < _directions.Count; i++)
		{
			yield return GetConnections(_directions.SkipAndLoop(i).Take(takeCount));
		}
	}

	private string GetConnections(IEnumerable<string> directions)
	{
		var connectionsAvailable = "";
		foreach (var direction in directions)
		{
			connectionsAvailable += HasExit(direction, "L");
			connectionsAvailable += HasExit(direction, "R");
		}

		return connectionsAvailable;
	}

	private int HasExit(string direction, string side)
	{
		var exitR = _getExit(string.Format("Exit_{0}_{1}", direction, side));
		return exitR != null ? 1 : 0;
	}
}