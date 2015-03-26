using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MachineCollision : MonoBehaviour
{
	public GameObject TileObjects;

	public void Start()
	{
	}

	void OnTriggerEnter(Collider other)
	{
		var collidedName = other.gameObject.name.ToUpper();
		if (collidedName.Contains("TREE"))
		{
			AddTreeFallAnimation(other.gameObject);
		}
		else if(CollidedIsOneOfFollowing(collidedName, new List<string>() { "HOUSE", "SKYSCRAPER", "COMPLEX", "MUNICIPAL", "GARAGE"}))
		{
			AddBuildingCrumbleAnimation(other.gameObject);
		}
	}

	private bool CollidedIsOneOfFollowing(string collidedName, List<string> names)
	{
		return names.Any(collidedName.Contains);
	}

	private void AddTreeFallAnimation(GameObject collidedGameObject)
	{
		if(collidedGameObject.GetComponent<TreeFall>() == null)
		{
			collidedGameObject.AddComponent<TreeFall>();
		}
	}

	private void AddBuildingCrumbleAnimation(GameObject collidedGameObject)
	{
		if (collidedGameObject.GetComponent<HouseCrash>() == null)
		{
			collidedGameObject.AddComponent<HouseCrash>();
		}
	}
}