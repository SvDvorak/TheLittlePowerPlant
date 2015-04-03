using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour
{
	public Vector3 TargetPosition;
	public Vector3 TargetMovement;
	public float MissileSpeed;
	public float TurnDegrees;
	private Vector3 _leadingTargetPosition;

	void Start ()
	{
		var secondsTillImpact = (TargetPosition - transform.position).magnitude/MissileSpeed;
		_leadingTargetPosition = TargetPosition + TargetMovement*secondsTillImpact;
	}

	void Update ()
	{
		var forwardDirection = transform.rotation * Vector3.forward;
		var targetDirection = (_leadingTargetPosition - transform.position).normalized;
		var completeAdjustRotation = Quaternion.FromToRotation(forwardDirection, targetDirection);
		Vector3 axis;
		float angle;
		completeAdjustRotation.ToAngleAxis(out angle, out axis);
		var partialAdjustRotation = Quaternion.AngleAxis(Mathf.Min(TurnDegrees, angle) * Time.deltaTime, axis);

		transform.rotation = partialAdjustRotation * transform.rotation;
		transform.position += transform.rotation * Vector3.forward * MissileSpeed * Time.deltaTime;
	}
}