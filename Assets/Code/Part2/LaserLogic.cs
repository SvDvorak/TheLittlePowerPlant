using UnityEngine;
using System.Collections;

public class LaserLogic : MonoBehaviour
{
    private Vector3 _target;

    void Start ()
	{
	
	}

	void Update ()
	{
	    if (_target != null)
	    {
            var ray = new Ray(transform.position, _target - transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var startToEnd = hit.point - transform.position;

                var localScale = transform.localScale;
                localScale.z = startToEnd.magnitude;
                transform.localScale = localScale;
                transform.rotation = Quaternion.LookRotation(-startToEnd, Vector3.up);
            }
        }
    }

    public void FireAt(Vector3 target)
    {
        _target = target;
    }
}