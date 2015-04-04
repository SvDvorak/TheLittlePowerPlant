using UnityEngine;
using System.Collections;
using Assets.Code;

public class MissileCollision : MonoBehaviour
{
	public float MissileDamage;

	void OnTriggerEnter(Collider other)
	{
		var collidedName = other.gameObject.name.ToUpper();
		if (collidedName.Contains("TRANSFORMER"))
		{
			var dataContext = other.gameObject.GetDataContext<GameState>();

			dataContext.Health -= MissileDamage;
		}

		Destroy(gameObject);
	}
}