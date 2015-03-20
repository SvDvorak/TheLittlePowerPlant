using UnityEngine;

public class Generation : MonoBehaviour, IBlockFactory
{
	private CityGeneration _cityGeneration;
	public GameObject BlockPrefab1;
	public GameObject BlockPrefab2;
	public GameObject BlockPrefab3;
	public GameObject BlockPrefab4;
	public GameObject BlockPrefab5;
	public GameObject BlockPrefab6;
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
		var collection = new TwoDimensionalCollection<TileInstance>();
		var coordinateTransformer = new Unity3DCoordinateTransformer();
		coordinateTransformer.SetOrigo(new Vector3(-NrOfTiles, 0));
		_cityGeneration = new CityGeneration(
			this,
			new TileAligner(
				new ConnectionsFinder(new UnityConnectionsRetriever()),
				new Random(),
				collection),
			collection,
			new UnityLogger(),
			coordinateTransformer);

		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
		_cityGeneration.SetTiles(new[] { BlockPrefab1, BlockPrefab2, BlockPrefab3, BlockPrefab4, BlockPrefab5, BlockPrefab6 });
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