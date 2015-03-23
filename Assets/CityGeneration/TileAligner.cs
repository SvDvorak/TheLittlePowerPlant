using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITileAligner
{
	void SetTiles(IEnumerable<object> tiles);
	TileInstance GetAlignedTile(int x, int y);
}

public class TileAligner : ITileAligner
{
	private readonly IConnectionsFinder _connectionsFinder;
	private readonly IEdgeConnections _edgeConnections;
	private readonly IRandom _random;
	private readonly NonStupidLookup<string, TileTemplate> _connectionToTilesMapping;

	public TileAligner(
		IConnectionsFinder connectionsFinder,
		IEdgeConnections edgeConnections,
		IRandom random)
	{
		_connectionsFinder = connectionsFinder;
		_edgeConnections = edgeConnections;
		_random = random;
		_connectionToTilesMapping = new NonStupidLookup<string, TileTemplate>();
	}

	public void SetTiles(IEnumerable<object> tiles)
	{
		foreach (var tile in tiles)
		{
			var connectionSets = _connectionsFinder.FindConnectionSets(tile);

			foreach (var connectionSet in connectionSets)
			{
				_connectionToTilesMapping.Append(connectionSet.Connections, new TileTemplate(tile, connectionSet.Rotation));
			}
		}
	}

	public TileInstance GetAlignedTile(int x, int y)
	{
		var requiredConnection = _edgeConnections.GetEdgeConnections(x, y);

		var possibleTemplates = requiredConnection.Connections != "" ? GetTileMapping(requiredConnection) : GetRandomTiles();
		var selectedTemplate = possibleTemplates[_random.Range(0, possibleTemplates.Count())];

		return CreateTileInstance(selectedTemplate, requiredConnection);
	}

	private List<TileTemplate> GetRandomTiles()
	{
		var keyGroupIndex = _random.Range(0, _connectionToTilesMapping.GetKeyGroupCount());
		return _connectionToTilesMapping.GetKeyGroupByIndex(keyGroupIndex).ToList();
	}

	private List<TileTemplate> GetTileMapping(ConnectionSet requiredConnection)
	{
		if (!_connectionToTilesMapping.HasKey(requiredConnection.Connections))
		{
			throw new NoTileWithConnections(requiredConnection.Connections);
		}
		return _connectionToTilesMapping[requiredConnection.Connections].ToList();
	}

	private TileInstance CreateTileInstance(TileTemplate selectedTemplate, ConnectionSet requiredConnection)
	{
		var rotation = DirectionRotation.NormalizeRotation(selectedTemplate.Rotation + requiredConnection.Rotation);
		var completeConnections = _connectionsFinder.GetCompleteConnectionsOriented(selectedTemplate.Tile, rotation);
		return new TileInstance(selectedTemplate.Tile, completeConnections, rotation);
	}
}