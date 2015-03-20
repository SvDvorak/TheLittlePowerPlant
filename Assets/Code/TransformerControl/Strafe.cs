using UnityEngine;
using System.Collections;

public class Strafe : MonoBehaviour
{
	public float DirectionChangeSpeedPerSecond;
	public float MovementSpeed;
	public float StrafeSpeed;

	private Animator _animator;
	private Vector3 _targetVector;
	private float _currentDirectionX;

	void Start()
	{
		_animator = GetComponent<Animator>();
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				_targetVector = hit.point - transform.position;
			}
		}

		_currentDirectionX = Mathf.MoveTowards(_currentDirectionX, Mathf.Clamp(-_targetVector.x/15, -1, 1), DirectionChangeSpeedPerSecond*Time.deltaTime);
		_animator.SetFloat("Strafe", _currentDirectionX);
		var movement = -Vector3.forward*MovementSpeed - new Vector3(_currentDirectionX, 0, 0)*StrafeSpeed;
		transform.position += movement*Time.deltaTime;
	}
}