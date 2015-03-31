using UnityEngine;
using Assets.Code;

public class Movement : MonoBehaviour
{
	public float DirectionChangeSpeedPerSecond;
	public float MovementSpeed;
	public float StrafeSpeed;
	public CameraShake CameraShake;

	private Animator _animator;
	private Vector3 _targetVector;
	private float _currentDirectionX;
	private GameState _gameState;
	private LTDescr _speedDecreaseTween;
	private float _movementMultiplier = 1;

	void Start()
	{
		_gameState = gameObject.GetDataContext<GameState>();
		_animator = GetComponent<Animator>();
	}

	void Update ()
	{
		if (_gameState.IsAlive && Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				_targetVector = hit.point - transform.position;
			}
		}

		if (!_gameState.IsAlive && _speedDecreaseTween == null)
		{
			_speedDecreaseTween = LeanTween.value(gameObject, value => _movementMultiplier = value, 1f, 0, _gameState.CrashTime);
			Invoke("GroundSmashCollision", 0.5f);
		}

		_currentDirectionX = Mathf.MoveTowards(_currentDirectionX, Mathf.Clamp(-_targetVector.x/15, -1, 1), DirectionChangeSpeedPerSecond*Time.deltaTime);
		_animator.SetFloat("Strafe", _currentDirectionX);
		var movement = -Vector3.forward*MovementSpeed - new Vector3(_currentDirectionX, 0, 0)*StrafeSpeed;
		transform.position += movement*_movementMultiplier*Time.deltaTime;
	}

	void GroundSmashCollision()
	{
		CameraShake.AddCollisionForce(2f);
	}
}