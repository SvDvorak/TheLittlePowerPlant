using UnityEngine;

public class HelicopterAI : MonoBehaviour
{
	public GameObject AttackTarget;
	public float FlySpeed;
	public float TargetKeepDistance;

	void Start ()
	{
	}

	void Update ()
	{
		var helicopterToTarget = AttackTarget.transform.position - transform.position;
		helicopterToTarget.y = 0;
		var normalizedToTarget = helicopterToTarget.normalized;
		var helicopterToTargetOffset = helicopterToTarget - normalizedToTarget*TargetKeepDistance;

		transform.position += helicopterToTargetOffset.normalized*FlySpeed*Time.deltaTime;
		transform.LookAt(helicopterToTarget + transform.position);
	}
}
