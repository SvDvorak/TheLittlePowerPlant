using System.Collections.Generic;
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
	public GameObject FollowObject;

	private readonly Dictionary<Vector3, GameObject> _createdTiles = new Dictionary<Vector3, GameObject>(); 

	public object Create(object block, Vector3 position, Vector3 rotation)
	{
		var newBlock = Instantiate((GameObject)block);
		newBlock.transform.position = position;
		newBlock.transform.Rotate(rotation);
		_createdTiles.Add(position, newBlock);

		return newBlock;
	}

	public void Destroy(Vector3 position)
	{
		Destroy(_createdTiles[position]);
		_createdTiles.Remove(position);
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
				new ConnectionRequirementsRetriever(collection), 
				new Random()),
			collection,
			new UnityLogger(),
			coordinateTransformer);

		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
		_cityGeneration.SetTiles(new[] { BlockPrefab1, BlockPrefab2, BlockPrefab3, BlockPrefab4, BlockPrefab5, BlockPrefab6 });
	}

	private void Update()
	{
		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
		_cityGeneration.Generate(FollowObject.transform.position);
	}
}