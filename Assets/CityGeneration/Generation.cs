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
		_connectionsFinder = new ConnectionsFinder();
	}

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
		var connections = _connectionsFinder.FindConnections(childName => BlockPrefab.transform.FindChild(name));
		var newBlock = Instantiate(BlockPrefab);
		newBlock.transform.position = position;
		newBlock.transform.Rotate(rotation);
	}
}