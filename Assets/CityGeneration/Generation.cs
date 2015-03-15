using UnityEngine;

public class Generation : MonoBehaviour, IBlockFactory
{
	private CityGeneration _cityGeneration;
	public GameObject BlockPrefab1;
	public GameObject BlockPrefab2;
	public GameObject BlockPrefab3;
	public GameObject BlockPrefab4;
	public int NrOfTiles;
	public int TileDimension;

	public object Create(object block, Vector3 position, Vector3 rotation)
	{
		var newBlock = Instantiate((GameObject)block);
		newBlock.transform.position = position;
		newBlock.transform.Rotate(rotation);
		return newBlock;
	}

	private void Start()
	{
		var collection = new TwoDimensionalCollection<PlacedTile>();
		_cityGeneration = new CityGeneration(
			this,
			new TileSelector(
				new ConnectionsFinder(new UnityConnectionsRetriever()),
				new Random(),
				collection),
			collection,
			new UnityLogger());

		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
		_cityGeneration.SetTiles(new[] { BlockPrefab1, BlockPrefab2, BlockPrefab3, BlockPrefab4 });
		_cityGeneration.Generate();
	}

	private void Update()
	{
		//CreateNewBlocks();
		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
	}

	private void CreateNewBlocks()
	{
	}
}

internal class UnityLogger : ILogger
{
	public void LogWarning(string message)
	{
		Debug.LogWarning(message);
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