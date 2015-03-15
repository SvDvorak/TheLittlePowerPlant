using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITileSelector
{
	void SetTiles(IEnumerable<object> tiles);
	TileInstance Select(int x, int y);
}

public class TileSelector : ITileSelector
{
	private readonly IConnectionsFinder _connectionsFinder;
	private readonly IRandom _random;
	private readonly NonStupidLookup<string, TileTemplate> _connectionToTilesMapping;
	private readonly ITwoDimensionalCollection<TileInstance> _placedTiles;

	private const int FlipRotation = 2;

	public TileSelector(
		IConnectionsFinder connectionsFinder,
		IRandom random,
		ITwoDimensionalCollection<TileInstance> placedTiles)
	{
		_connectionsFinder = connectionsFinder;
		_random = random;
		_connectionToTilesMapping = new NonStupidLookup<string, TileTemplate>();
		_placedTiles = placedTiles;
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

	public TileInstance Select(int x, int y)
	{
		var requiredConnections = "";
		int rotationToRequiredConnections = 0;

		var leftTile = _placedTiles[x - 1, y];
		if (leftTile != null)
		{
			rotationToRequiredConnections = 1 + FlipRotation;
			requiredConnections += GetOppositeSide(GetSideConnections(leftTile.AllConnections, 1 - leftTile.Rotation));
		}
		var topTile = _placedTiles[x, y - 1];
		if (topTile != null)
		{
			rotationToRequiredConnections = requiredConnections == null ? 2 + FlipRotation : rotationToRequiredConnections;
			requiredConnections += GetOppositeSide(GetSideConnections(topTile.AllConnections, 2 - topTile.Rotation));
		}

		List<TileTemplate> possibleTemplates;
		if (requiredConnections != "")
		{
			if (!_connectionToTilesMapping.HasKey(requiredConnections))
			{
				throw new NoTileWithConnections(requiredConnections);
			}
			possibleTemplates = _connectionToTilesMapping[requiredConnections].ToList();
		}
		else
		{
			var keyGroupIndex = _random.Range(0, _connectionToTilesMapping.GetKeyGroupCount());
			possibleTemplates = _connectionToTilesMapping.GetKeyGroupByIndex(keyGroupIndex).ToList();
		}
		var selectedTemplate = possibleTemplates[_random.Range(0, possibleTemplates.Count())];

		int rotation = GetNormalizedRotation(selectedTemplate.Rotation + rotationToRequiredConnections);
		var completeConnections = _connectionsFinder.GetCompleteConnectionsOriented(selectedTemplate.Tile, rotation);
		return new TileInstance(selectedTemplate.Tile, completeConnections, rotation);
	}

	private string GetSideConnections(string allConnections, int rotation)
	{
		rotation = GetNormalizedRotation(rotation);
		return allConnections.Substring(rotation*2, 2);
	}

	private static int GetNormalizedRotation(int rotation)
	{
		rotation = rotation%4;
		rotation = rotation < 0 ? 4 + rotation : rotation;
		return rotation;
	}

	private string GetOppositeSide(string sideConnections)
	{
		return sideConnections.Substring(1, 1) + sideConnections.Substring(0, 1);
	}
}