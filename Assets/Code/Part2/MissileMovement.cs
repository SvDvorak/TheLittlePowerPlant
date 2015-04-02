using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour
{
	public Transform Target;
	public float MissileSpeed;
	public float TurnDegrees;
	private Vector3 _spawnPosition;
	private Vector3 _spawnToTargetNormalized;

	void Start ()
	{
		_spawnPosition = transform.position;
		_spawnToTargetNormalized = (Target.position - _spawnPosition).normalized;
	}

	void Update ()
	{
		var forwardDirection = transform.rotation*Vector3.forward;
		var targetDirection = (Target.position - transform.position).normalized;
		var completeAdjustRotation = Quaternion.FromToRotation(forwardDirection, targetDirection);
		Vector3 axis;
		float angle;
		completeAdjustRotation.ToAngleAxis(out angle, out axis);
		var partialAdjustRotation = Quaternion.AngleAxis(Mathf.Min(TurnDegrees, angle) * Time.deltaTime, axis);

		transform.rotation = partialAdjustRotation*transform.rotation;
		transform.position += transform.rotation*Vector3.forward*MissileSpeed*Time.deltaTime;
	}
}