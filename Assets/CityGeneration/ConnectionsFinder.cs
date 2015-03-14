using System;
using System.Collections.Generic;
using System.Linq;

public interface IConnectionsFinder
{
	IEnumerable<ConnectionSet> FindConnectionSets(object tile);
	string GetCompleteConnectionsOriented(object tile, int rotation);
}

public class ConnectionSet
{
	public string Connections;
	public int Rotation;

	public ConnectionSet(string connections, int rotation)
	{
		Connections = connections;
		Rotation = rotation;
	}
}

public class ConnectionsFinder : IConnectionsFinder
{
	private readonly IExitRetriever _exitRetriever;
	private readonly List<string> _directions;
	private object _tile;
	private const int QuarterRotation = 90;

	public ConnectionsFinder(IExitRetriever exitRetriever)
	{
		_exitRetriever = exitRetriever;
		_directions = new List<string> { "N", "E", "S", "W" };
	}

	public IEnumerable<ConnectionSet> FindConnectionSets(object tile)
	{
		_tile = tile;
		return SubdivideAndCalculate().ToList();
	}

	public string GetCompleteConnectionsOriented(object tile, int rotation)
	{
		_tile = tile;
		return GetConnections(_directions, rotation).Connections;
	}

	private IEnumerable<ConnectionSet> SubdivideAndCalculate()
	{
		var connections = new List<ConnectionSet>();
		for (var subdivideCount = _directions.Count; subdivideCount > 0; subdivideCount--)
		{
			connections.AddRange(RotateAndCalculate(subdivideCount));
		}
		return connections;
	}

	private IEnumerable<ConnectionSet> RotateAndCalculate(int takeCount)
	{
		for (var i = 0; i < _directions.Count; i++)
		{
			yield return GetConnections(_directions.SkipAndLoop(i).Take(takeCount), i*QuarterRotation);
		}
	}

	private ConnectionSet GetConnections(IEnumerable<string> directions, int rotation)
	{
		var connectionsAvailable = "";
		foreach (var direction in directions)
		{
			connectionsAvailable += HasExit(direction, "L");
			connectionsAvailable += HasExit(direction, "R");
		}

		return new ConnectionSet(connectionsAvailable, rotation);
	}

	private int HasExit(string direction, string side)
	{
		var exitR = _exitRetriever.GetExits(_tile, string.Format("Exit_{0}_{1}", direction, side));
		return exitR != null ? 1 : 0;
	}
}