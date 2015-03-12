using System;
using UnityEngine;

public class CityGeneration
{
	public float NrOfTiles { get; set; }
	public float TileDimension { get; set; }

	private readonly IRandom _random;
	private readonly IBlockFactory _blockFactory;

	public CityGeneration(IRandom random, IBlockFactory blockFactory)
	{
		_random = random;
		_blockFactory = blockFactory;
	}

	public void Generate()
	{
		for (int x = 0; x < NrOfTiles; x++)
		{
			for (int z = 0; z < NrOfTiles; z++)
			{
				_blockFactory.Create(new Vector3(x * TileDimension, 0, z * TileDimension), new Vector3(0, 90 * _random.Range(0, 4)));
			}
		}
	}
}

public interface IBlockFactory
{
	void Create(Vector3 position, Vector3 rotation);
}