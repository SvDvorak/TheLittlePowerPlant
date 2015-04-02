using UnityEngine;

public class HelicopterAI : MonoBehaviour
{
	public GameObject AttackTarget;
	public float FlySpeed;
	public float MaxSpeedDistance;
	public float TargetMinDistance;
	public float TargetMaxDistance;
	public float NewOffsetTimeInSeconds;
	public float OffsetConeFromTarget;
	public float FireDelay;
	public GameObject MissileTemplate;
	public Transform MissileSpawnL;
	public Transform MissileSpawnR;

	private Vector3 _offset;
	private bool _fireFromLeft;

	void Start ()
	{
		InvokeRepeating("SetNewRandomOffset", 0, NewOffsetTimeInSeconds);
		InvokeRepeating("FireMissile", FireDelay, FireDelay);
	}

	void Update ()
	{
		var helicopterToTargetOffset = (_offset + AttackTarget.transform.position) - transform.position;
		helicopterToTargetOffset.y = 0;

		var velocity = Mathf.SmoothStep(0, FlySpeed, helicopterToTargetOffset.magnitude/MaxSpeedDistance);
		transform.position += helicopterToTargetOffset.normalized*velocity*Time.deltaTime;
		transform.LookAt(AttackTarget.transform.position);
	}

	private void SetNewRandomOffset()
	{
		var randomDirection = Quaternion.Euler(0, UnityEngine.Random.Range(-OffsetConeFromTarget, OffsetConeFromTarget), 0)*(-Vector3.forward);
		_offset = randomDirection*UnityEngine.Random.Range(TargetMinDistance, TargetMaxDistance);
	}

	[ContextMenu("Spawn Missile")]
	private void FireMissile()
	{
		if (_fireFromLeft)
		{
			SpawnMissile(MissileSpawnL);
		}
		else
		{
			SpawnMissile(MissileSpawnR);
		}

		_fireFromLeft = !_fireFromLeft;
	}

	private void SpawnMissile(Transform missileSpawn)
	{
		var missile = Instantiate(MissileTemplate);
		missile.transform.position = missileSpawn.position;
		var missileMovement = missile.GetComponent<MissileMovement>();
		missileMovement.Target = AttackTarget.transform;
	}
}
