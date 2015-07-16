using UnityEngine;

public class TreeFall : MonoBehaviour
{
	private const float MaxTime = 1;
	private float _animationTime;

	public void Start ()
	{
		_animationTime = 0;
	}

	public void Update()
	{
        if(_animationTime < MaxTime)
        {
            transform.RotateAround(transform.position, Vector3.left, (Time.deltaTime*90)/MaxTime);
        }
		_animationTime += Time.deltaTime;
	}
}