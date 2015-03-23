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
	private readonly IConnectionRequirementsRetriever _connectionRequirementsRetriever;
	private readonly IRandom _random;
	private readonly NonStupidLookup<string, TileTemplate> _connectionToTilesMapping;

	public TileAligner(
		IConnectionsFinder connectionsFinder,
		IConnectionRequirementsRetriever connectionRequirementsRetriever,
		IRandom random)
	{
		_connectionsFinder = connectionsFinder;
		_connectionRequirementsRetriever = connectionRequirementsRetriever;
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
		var requiredConnection = _connectionRequirementsRetriever.GetRequiredConnection(x, y);

		List<TileTemplate> possibleTemplates;
		if (requiredConnection.Connections != "")
		{
			if (!_connectionToTilesMapping.HasKey(requiredConnection.Connections))
			{
				throw new NoTileWithConnections(requiredConnection.Connections);
			}
			possibleTemplates = _connectionToTilesMapping[requiredConnection.Connections].ToList();
		}
		else
		{
			var keyGroupIndex = _random.Range(0, _connectionToTilesMapping.GetKeyGroupCount());
			possibleTemplates = _connectionToTilesMapping.GetKeyGroupByIndex(keyGroupIndex).ToList();
		}
		var selectedTemplate = possibleTemplates[_random.Range(0, possibleTemplates.Count())];

		var rotation = DirectionRotation.NormalizeRotation(selectedTemplate.Rotation + requiredConnection.Rotation);
		var completeConnections = _connectionsFinder.GetCompleteConnectionsOriented(selectedTemplate.Tile, rotation);
		return new TileInstance(selectedTemplate.Tile, completeConnections, rotation);
	}
}