using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityGeneration
{
	public float NrOfTiles { get; set; }
	public float TileDimension { get; set; }
	private readonly IBlockFactory _blockFactory;
	private readonly ITileSelector _tileSelector;
	private readonly ITwoDimensionalCollection<PlacedTile> _placedTiles;

	public CityGeneration(IBlockFactory blockFactory, ITileSelector tileSelector, ITwoDimensionalCollection<PlacedTile> placedTiles)
	{
		_blockFactory = blockFactory;
		_tileSelector = tileSelector;
		_placedTiles = placedTiles;
	}

	public void Generate()
	{
		for (int x = 0; x < NrOfTiles; x++)
		{
			for (int z = 0; z < NrOfTiles; z++)
			{
				var tileTemplate = _tileSelector.Select(x, z);
				_placedTiles[x, z] = tileTemplate;
				_blockFactory.Create(tileTemplate.Tile, new Vector3(x * TileDimension, 0, z * TileDimension), new Vector3(0, 90 * tileTemplate.Rotation));
			}
		}
	}

	public void SetTiles(IEnumerable<object> tiles)
	{
		_tileSelector.SetTiles(tiles);
	}
}

public interface IBlockFactory
{
	object Create(object block, Vector3 position, Vector3 rotation);
}