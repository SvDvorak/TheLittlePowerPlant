using UnityEngine;
using System.Collections;

public class Generation : MonoBehaviour, IBlockFactory
{
	private CityGeneration _cityGeneration;

	public float TileDimension;
	public float NrOfTiles;
	public GameObject BlockPrefab;
	private readonly ConnectionsFinder _connectionsFinder;

	public Generation()
	{
		_connectionsFinder = new ConnectionsFinder(new UnityConnectionsRetriever());
	}

	void Start ()
	{
		_cityGeneration = new CityGeneration(new Random(), this, null, null);
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

	public object Create(object block, Vector3 position, Vector3 rotation)
	{
		var newBlock = Instantiate(BlockPrefab);
		newBlock.transform.position = position;
		newBlock.transform.Rotate(rotation);
		return newBlock;
	}
}

public class UnityConnectionsRetriever : IExitRetriever
{
	public object GetExits(object tile, string name)
	{
		var gameObject = tile as GameObject;
		return gameObject.transform.FindChild(name);
	}
}