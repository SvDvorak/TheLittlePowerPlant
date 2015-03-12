using UnityEngine;
using System.Collections;

public class Generation : MonoBehaviour, IBlockFactory
{
	private CityGeneration _cityGeneration;

	public float TileDimension;
	public float NrOfTiles;
	public GameObject BlockPrefab;

	void Start ()
	{
		_cityGeneration = new CityGeneration(new Random(), this);
		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
		_cityGeneration.Generate();
	}

	void Update()
	{
		//CreateNewBlocks();
		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
	}

	private void CreateNewBlocks()
	{
	}

	public void Create(Vector3 position, Vector3 rotation)
	{
		var newBlock = Instantiate(BlockPrefab);
		newBlock.transform.position = position;
		newBlock.transform.Rotate(rotation);
	}
}

public interface IRandom
{
	int Range(int min, int max);
	float Range(float min, float max);
}

public class Random : IRandom
{
	public int Range(int min, int max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	public float Range(float min, float max)
	{
		return UnityEngine.Random.Range(min, max);
	}
}