using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code;

public class MachineCollision : MonoBehaviour
{
	public GameObject TileObjects;
	public CameraShake CameraShake;

	//public float TreeCollisionForce = 0.2f;
	//public float HouseCollisionForce = 0.5f;
	//public float SkyscraperCollisionForce = 1.5f;
	private GameState _dataContext;

	public void OnTriggerEnter(Collider other)
	{
        var damageable = other.transform.GetComponent(typeof(IDamageable)) as IDamageable;
        if (damageable != null)
        {
            damageable.DoDamage(float.PositiveInfinity);
            CameraShake.AddCollisionForce(damageable.InitialHealth/10f);
            MachineDamage(damageable.InitialHealth/100f);
        }
    }

	private void MachineDamage(float amount)
	{
	    if (amount < 0.1f)
	    {
	        return;
	    }

	    var dataContext = GetDataContext();
	    dataContext.Health -= amount;
	    dataContext.GotHit = true;
	    GetComponent<Animator>().SetTrigger("Hit");
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