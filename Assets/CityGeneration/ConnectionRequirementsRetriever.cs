using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IConnectionRequirementsRetriever
{
	ConnectionSet GetRequiredConnection(int x, int y);
}

public class ConnectionRequirementsRetriever : IConnectionRequirementsRetriever
{
	private readonly ITwoDimensionalCollection<TileInstance> _tiles;

	private readonly List<Vector3> _surroundingDirections = new List<Vector3>
		{
			new Vector3(0, -1),
			new Vector3(1, 0),
			new Vector3(0, 1),
			new Vector3(-1, 0)
		};

	private const int EdgeConnectionCount = 2;
	private const int FlipRotation = 2;

	public ConnectionRequirementsRetriever(ITwoDimensionalCollection<TileInstance> tiles)
	{
		_tiles = tiles;
	}

	public ConnectionSet GetRequiredConnection(int x, int y)
	{
		var surroundingTiles = _surroundingDirections.Select(delta => _tiles[x + (int)delta.x, y + (int)delta.y]).ToList();
		var longestRequirement = "";
		var currentRequirement = "";
		var rotationToRequiredConnections = 0;

		for (var i = 0; i < _surroundingDirections.Count * 2; i++)
		{
			var currentTile = surroundingTiles[i % _surroundingDirections.Count];
			if (currentTile != null)
			{
				var rotationToConnectingSide = i%_surroundingDirections.Count + FlipRotation - currentTile.Rotation;
				currentRequirement += GetOppositeSide(GetSideConnections(currentTile.AllConnections, rotationToConnectingSide));
			}
			else
			{
				currentRequirement = "";
			}

			if (currentRequirement.Length > longestRequirement.Length)
			{
				longestRequirement = currentRequirement;
				var currentTileToFirstRotationOffset = currentRequirement.Length/EdgeConnectionCount-1;
				rotationToRequiredConnections = DirectionRotation.NormalizeRotation(i%_surroundingDirections.Count - currentTileToFirstRotationOffset);
			}

			if (currentRequirement.Length == _surroundingDirections.Count*EdgeConnectionCount)
			{
				break;
			}
		}

		return new ConnectionSet(longestRequirement, rotationToRequiredConnections);
	}

	private static string GetSideConnections(string allConnections, int rotation)
	{
		return allConnections.Substring(DirectionRotation.NormalizeRotation(rotation) * EdgeConnectionCount, EdgeConnectionCount);
	}

	private static string GetOppositeSide(string sideConnections)
	{
		return sideConnections.Substring(1, 1) + sideConnections.Substring(0, 1);
	}

}