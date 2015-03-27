﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MachineCollision : MonoBehaviour
{
	public GameObject TileObjects;
	public CameraShake CameraShake;

	public float TreeCollisionForce = 0.2f;
	public float HouseCollisionForce = 0.5f;
	public float SkyscraperCollisionForce = 1.5f;

	void OnTriggerEnter(Collider other)
	{
		var collidedName = other.gameObject.name.ToUpper();
		if (collidedName.Contains("TREE"))
		{
			AddTreeFallAnimation(other.gameObject);
			CameraShake.AddCollisionForce(TreeCollisionForce);
		}
		else if (CollidedIsOneOfFollowing(collidedName,
			new List<string>() { "HOUSE" }))
		{
			AddBuildingCrumbleAnimation(other.gameObject);
			CameraShake.AddCollisionForce(HouseCollisionForce);
		}
		else if(CollidedIsOneOfFollowing(collidedName, new List<string>() { "SKYSCRAPER", "COMPLEX", "MUNICIPAL", "GARAGE"}))
		{
			AddBuildingCrumbleAnimation(other.gameObject);
			CameraShake.AddCollisionForce(SkyscraperCollisionForce);
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