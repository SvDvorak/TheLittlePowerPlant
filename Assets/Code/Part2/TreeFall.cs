using UnityEngine;

public class TreeFall : MonoBehaviour
{
	private const float MaxTime = 1;
	private float _animationTime;
	private Vector3 _startingOrientation;

	public void Start ()
	{
		_animationTime = 0;
		_startingOrientation = transform.eulerAngles;
	}

	public void Update()
	{
		transform.eulerAngles = _startingOrientation + new Vector3(Mathf.Clamp(_animationTime/MaxTime, 0, 1)*90, 0, 0);
		_animationTime += Time.deltaTime;
	}
}