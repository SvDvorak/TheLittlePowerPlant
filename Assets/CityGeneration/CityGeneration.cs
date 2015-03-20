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

	public void Generate(Vector3 centerPoint)
	{
		centerPoint = _transformer.Transform(centerPoint);
		var tileCenter = new Vector3((int)(centerPoint.x/TileDimension), (int)(centerPoint.y/TileDimension));

		for (var y = -NrOfTiles - 2; y < NrOfTiles + 2; y++)
		{
			for (var x = -NrOfTiles - 2; x < NrOfTiles + 2; x++)
			{
				var centerAdjustedX = x + (int)tileCenter.x;
				var centerAdjustedY = y + (int)tileCenter.y;
				var possibleTile = _placedTiles[centerAdjustedX, centerAdjustedY];
				if (!IsInside(x, y) && possibleTile != null)
				{
					var position = new Vector3(centerAdjustedX * TileDimension, centerAdjustedY * TileDimension, 0);
					_blockFactory.Destroy(_transformer.Transform(position));
					_placedTiles[centerAdjustedX, centerAdjustedY] = null;
				}
			}
		}

		for (var y = -NrOfTiles - 1; y <= NrOfTiles; y++)
		{
			for (var x = -NrOfTiles - 1; x < NrOfTiles; x++)
			{
				var centerAdjustedX = x + (int)tileCenter.x;
				var centerAdjustedY = y + (int)tileCenter.y;
				if(IsInside(x, y) && _placedTiles[centerAdjustedX, centerAdjustedY] == null)
				{
					TryPlaceTile(centerAdjustedX, centerAdjustedY);
				}
			}
		}
	}

	private bool IsInside(int x, int y)
	{
		return Math.Abs(x) + Math.Abs(y) < NrOfTiles;
	}

	private void TryPlaceTile(int x, int y)
	{
		try
		{
			var tileTemplate = _tileAligner.GetAlignedTile(x, y);
			_placedTiles[x, y] = tileTemplate;
			var position = new Vector3(x*TileDimension, y* TileDimension, 0);
			_blockFactory.Create(tileTemplate.Tile, _transformer.Transform(position), new Vector3(0, 90*tileTemplate.Rotation));
		}
		catch (NoTileWithConnections exception)
		{
			_logger.LogWarning(exception.Message);
		}
	}

	public void SetTiles(IEnumerable<object> tiles)
	{
		_tileAligner.SetTiles(tiles);
	}
}