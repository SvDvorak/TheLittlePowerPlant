using System;
using UnityEngine;
using System.Collections;
using Assets.Code;

public class LaserLogic : MonoBehaviour
{
    public float Damage;
    private Vector3 _target;
    private Vector3 _initialScale;

    public void Start()
    {
        _initialScale = transform.lossyScale;
    }

    public void Update ()
	{
	    var ray = new Ray(transform.position, _target - transform.position);
	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit))
	    {
	        var startToEnd = hit.point - transform.position;

	        var localScale = transform.localScale;
            localScale.z = startToEnd.magnitude*(1/_initialScale.z);
            transform.localScale = localScale;
	        transform.rotation = Quaternion.LookRotation(-startToEnd, Vector3.up);

            var damageable = hit.transform.GetComponent(typeof(IDamageable)) as IDamageable;

            if (damageable != null)
            {
                damageable.DoDamage(Time.deltaTime * Damage);
            }
	    }
    }

    public void FireAt(Vector3 target)
    {
        gameObject.SetActive(true);
        _target = target;
    }

    public void StopFiring()
    {
        gameObject.SetActive(false);
    }
}