﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityGenerationUnityComponent : MonoBehaviour, IBlockFactory
{
	private CityGeneration _cityGeneration;

	public List<GameObject> Tiles = new List<GameObject>(); 
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

	    foreach (Transform item in newBlock.transform)
	    {
	        var nameInUpper = item.name.ToUpper();
	        if (NameIsOneOfFollowing(nameInUpper, new[] { "TREE" }))
	        {
	            item.gameObject.AddComponent<Destroyable>().Init(0.1f, 0.3f, typeof(TreeFall));
	        }
            else if (NameIsOneOfFollowing(nameInUpper, new[] { "HOUSE" }))
            {
                item.gameObject.AddComponent<Destroyable>().Init(1, 3, typeof(HouseCrash));
            }
            else if (NameIsOneOfFollowing(nameInUpper, new[] { "SKYSCRAPER", "COMPLEX", "MUNICIPAL", "GARAGE" }))
            {
                item.gameObject.AddComponent<Destroyable>().Init(100, 10, typeof(HouseCrash));
            }
	    }

	    return newBlock;
	}

    private bool NameIsOneOfFollowing(string actualName, string[] nameCollection)
    {
        return nameCollection.Any(actualName.Contains);
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
				new EdgeConnections(collection), 
				new Random()),
			collection,
			new UnityLogger(),
			coordinateTransformer);

		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
		_cityGeneration.SetTiles(Tiles.Cast<object>());
	}

	private void Update()
	{
		_cityGeneration.TileDimension = TileDimension;
		_cityGeneration.NrOfTiles = NrOfTiles;
		_cityGeneration.Generate(FollowObject.transform.position);
	}
}