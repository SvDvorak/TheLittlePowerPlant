using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityGeneration
{
	public int NrOfTiles { get; set; }
	public int TileDimension { get; set; }
	private readonly IBlockFactory _blockFactory;
	private readonly ITileSelector _tileSelector;
	private readonly ITwoDimensionalCollection<PlacedTile> _placedTiles;
	private readonly ILogger _logger;

	public CityGeneration(IBlockFactory blockFactory, ITileSelector tileSelector, ITwoDimensionalCollection<PlacedTile> placedTiles, ILogger logger)
	{
		_blockFactory = blockFactory;
		_tileSelector = tileSelector;
		_placedTiles = placedTiles;
		_logger = logger;
	}

	public void Generate()
	{
		for (int z = 0; z < NrOfTiles; z++)
		{
			for (int x = NrOfTiles; x > 0; x--)
			{
				try
				{
					var tileTemplate = _tileSelector.Select(x, z);
					_placedTiles[x, z] = tileTemplate;
					_blockFactory.Create(tileTemplate.Tile, new Vector3(x * TileDimension, 0, z * TileDimension), new Vector3(0, 90 * tileTemplate.Rotation));
				}
				catch (NoTileWithConnections exception)
				{
					_logger.LogWarning(exception.Message);
				}
			}
		}
	}

	public void SetTiles(IEnumerable<object> tiles)
	{
		_tileSelector.SetTiles(tiles);
	}
}

public interface ILogger
{
	void LogWarning(string message);
}

public interface IBlockFactory
{
	object Create(object block, Vector3 position, Vector3 rotation);
}