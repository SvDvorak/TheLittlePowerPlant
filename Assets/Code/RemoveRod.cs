using UnityEngine;
using System.Collections;

public class RemoveRod : MonoBehaviour
{
	private const float FrictionMultiplier = 0.8f;
	public float TopYPosition = 0;
	public float BottomYPosition = 0;


	private bool _isLocked;
	private float _previousCameraY;

	void Start()
	{
		animation["LiftRod"].speed = 0;
		animation.Play();
	}

	void Update()
	{
		UpdateMouseLocking();

		if (_isLocked)
		{
			MoveRods();
		}
	}

	private void UpdateMouseLocking()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(0))
			{
				_previousCameraY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
				_isLocked = true;
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			_isLocked = false;
		}
	}

	private void MoveRods()
	{
		var currentCameraY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
		var verticalMovement = (currentCameraY - _previousCameraY) * (1 - FrictionMultiplier);
		//var newControlRodEffect = _nuclear.ControlRodEffect + verticalMovement / RodPositionRange;
		//_nuclear.ControlRodEffect = Mathf.Clamp(newControlRodEffect, 0, 1);

		//transform.localPosition = new Vector3(transform.localPosition.x, _nuclear.ControlRodEffect * RodPositionRange + BottomYPosition, transform.localPosition.z);
		animation["LiftRod"].time = animation["LiftRod"].time + verticalMovement;
		_previousCameraY = currentCameraY;
	}
}
