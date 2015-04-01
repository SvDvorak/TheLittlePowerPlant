using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour
{
	void Start ()
	{
	
	}
	
	void Update ()
	{
		transform.position += new Vector3(0, 0, 1*Time.deltaTime);
	}
}