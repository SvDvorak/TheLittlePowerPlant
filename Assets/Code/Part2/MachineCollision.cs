using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code;

public class MachineCollision : MonoBehaviour
{
	public GameObject TileObjects;
	public CameraShake CameraShake;

	public float TreeCollisionForce = 0.2f;
	public float HouseCollisionForce = 0.5f;
	public float SkyscraperCollisionForce = 1.5f;
	private GameState _dataContext;

	void OnTriggerEnter(Collider other)
	{
		var collidedName = other.gameObject.name.ToUpper();
		if (collidedName.Contains("TREE"))
		{
			AddTreeFallAnimation(other.gameObject);
			CameraShake.AddCollisionForce(TreeCollisionForce);
			DecreaseCityValue(0.1f);
		}
		else if (CollidedIsOneOfFollowing(collidedName,
			new List<string>() { "HOUSE" }))
		{
			AddBuildingCrumbleAnimation(other.gameObject);
			CameraShake.AddCollisionForce(HouseCollisionForce);
			DecreaseCityValue(1);
		}
		else if(CollidedIsOneOfFollowing(collidedName, new List<string>() { "SKYSCRAPER", "COMPLEX", "MUNICIPAL", "GARAGE"}))
		{
			AddBuildingCrumbleAnimation(other.gameObject);
			CameraShake.AddCollisionForce(SkyscraperCollisionForce);
			MachineDamage(0.1f);
			DecreaseCityValue(100);
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

	private void MachineDamage(float amount)
	{
		var dataContext = GetDataContext();
		dataContext.Health -= amount;
		dataContext.GotHit = true;
		var animator = GetComponent<Animator>();
		animator.SetTrigger("Hit");
	}

	private void DecreaseCityValue(float destructionCost)
	{
		GetDataContext().DestructionCost += destructionCost;
	}

	private GameState GetDataContext()
	{
		if (_dataContext == null)
		{
			_dataContext = gameObject.GetDataContext<GameState>();
		}

		return _dataContext;
	}
}