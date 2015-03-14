using System.Collections.Generic;
using System.Linq;

public interface ITileSelector
{
	void SetTiles(IEnumerable<object> tiles);
	PlacedTile Select(int x, int y);
}

public class TileSelector : ITileSelector
{
	private readonly IConnectionsFinder _connectionsFinder;
	private readonly NonStupidLookup<string, TileTemplate> _connectionToTilesMapping;
	private readonly TwoDimensionalCollection<PlacedTile> _placedTiles;

	private const int FlipRotation = 2;

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

	public PlacedTile Select(int x, int y)
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

		TileTemplate tileTemplate;
		if (requiredConnections != "")
		{
			tileTemplate = _connectionToTilesMapping[requiredConnections].First();
		}
		else
		{
			tileTemplate = _connectionToTilesMapping.GetTile();
		}

		PlaceTile(x, y, tileTemplate.Tile, tileTemplate.Rotation + rotationToRequiredConnections);
		return _placedTiles[x, y];
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


	public void PlaceTile(int x, int y, object tile, int rotation)
	{
		var completeConnections = _connectionsFinder.GetCompleteConnectionsOriented(tile, rotation);
		var placedTile = new PlacedTile(tile, completeConnections, GetNormalizedRotation(rotation));
		_placedTiles[x, y] = placedTile;
	}
}

public class PlacedTile
{
	public object Tile { get; private set; }
	public string AllConnections { get; private set; }
	public int Rotation { get; private set; }

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

	public object Tile { get; private set; }
	public int Rotation { get; private set; }
}