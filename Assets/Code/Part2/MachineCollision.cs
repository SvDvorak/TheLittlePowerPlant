using UnityEngine;
using System.Collections;

public class MachineCollision : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		Destroy(other.gameObject);
	}
}