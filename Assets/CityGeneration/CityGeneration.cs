using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityGeneration
{
	public int NrOfTiles { get; set; }
	public int TileDimension { get; set; }

	private readonly IBlockFactory _blockFactory;
	private readonly ITileAligner _tileAligner;
	private readonly ITwoDimensionalCollection<TileInstance> _placedTiles;
	private readonly ILogger _logger;
	private readonly ICoordinateTransformer _transformer;

	public CityGeneration(
		IBlockFactory blockFactory,
		ITileAligner tileAligner,
		ITwoDimensionalCollection<TileInstance> placedTiles,
		ILogger logger,
		ICoordinateTransformer transformer)
	{
		_blockFactory = blockFactory;
		_tileAligner = tileAligner;
		_placedTiles = placedTiles;
		_logger = logger;
		_transformer = transformer;
	}

	public void Generate()
	{
		for (int y = 0; y < NrOfTiles; y++)
		{
			for (int x = 0; x < NrOfTiles; x++)
			{
				try
				{
					var tileTemplate = _tileAligner.GetAlignedTile(x, y);
					_placedTiles[x, y] = tileTemplate;
					_blockFactory.Create(tileTemplate.Tile, _transformer.Transform(new Vector3(x * TileDimension, y * TileDimension, 0)), new Vector3(0, 90 * tileTemplate.Rotation));
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
		_tileAligner.SetTiles(tiles);
	}
}