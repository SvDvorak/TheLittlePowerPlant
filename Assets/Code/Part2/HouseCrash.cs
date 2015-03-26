using UnityEngine;
using System.Collections;

public class HouseCrash : MonoBehaviour
{
	private const float MovementPerSecond = 2;

	void Start ()
	{
	
	}
	
	void Update ()
	{
		transform.Translate(0, 0, -Time.deltaTime*MovementPerSecond);
	}
}