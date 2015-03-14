using System.Collections.Generic;
using System.Linq;

public interface ITileSelector
{
	void SetTiles(IEnumerable<object> tiles);
	object Select(int x, int z);
}

public class TileSelector : ITileSelector
{
	private readonly IConnectionsFinder _connectionsFinder;
	private readonly NonStupidLookup<string, TileTemplate> _connectionToTilesMapping;
	private readonly TwoDimensionalCollection<PlacedTile> _placedTiles;

	public TileSelector(IConnectionsFinder connectionsFinder)
	{
		_connectionsFinder = connectionsFinder;
		_connectionToTilesMapping = new NonStupidLookup<string, TileTemplate>();
		_placedTiles = new TwoDimensionalCollection<PlacedTile>();
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

	public object Select(int x, int z)
	{
		var requiredConnections = "";
		int? prevToConnection = null;
		var leftTile = _placedTiles[x - 1, z];
		if (leftTile != null)
		{
			prevToConnection = 1;
			requiredConnections += GetOppositeSide(GetSideConnections(leftTile.AllConnections, (5 - leftTile.Rotation) % 4));
		}
		var topTile = _placedTiles[x, z - 1];
		if (topTile != null)
		{
			prevToConnection = prevToConnection == null ? 2 : prevToConnection;
			requiredConnections += GetOppositeSide(GetSideConnections(topTile.AllConnections, (6 - topTile.Rotation) % 4));
		}

		TileTemplate tileTemplate;

		if (requiredConnections != "")
		{
			tileTemplate = _connectionToTilesMapping[requiredConnections].First();
		}
		else
		{
			tileTemplate = _connectionToTilesMapping.GetTile();
		}

		PlaceTile(x, z, tileTemplate, prevToConnection != null ? prevToConnection.Value + 2 : 0);
		return tileTemplate.Tile;
	}

	private string GetSideConnections(string allConnections, int rotation)
	{
		return (allConnections + allConnections + allConnections).Substring(rotation*2, 2);
	}

	private string GetOppositeSide(string sideConnections)
	{
		return sideConnections.Substring(1, 1) + sideConnections.Substring(0, 1);
	}


	public void PlaceTile(int x, int z, TileTemplate template, int extraRotation)
	{
		var completeConnections = _connectionsFinder.GetCompleteConnectionsOriented(template.Tile, template.Rotation);
		var placedTile = new PlacedTile(template.Tile, completeConnections, (template.Rotation + extraRotation) % 4);
		_placedTiles[x, z] = placedTile;
	}
}

public class PlacedTile
{
	public object Tile { get; }
	public string AllConnections { get; }
	public int Rotation { get; }

	public PlacedTile(object tile, string allConnections, int rotation)
	{
		Tile = tile;
		AllConnections = allConnections;
		Rotation = rotation;
	}
}

public class TileTemplate
{
	public TileTemplate(object tile, int rotation)
	{
		Tile = tile;
		Rotation = rotation;
	}

	public object Tile { get; }
	public int Rotation { get; }
}