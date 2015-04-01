using Assets.Code;
using UnityEngine;

public class DeathUpdater : MonoBehaviour
{
	public float CrashTime;

	private GameState _gameState;
	private Animator _animator;

	void Start ()
	{
		_gameState = gameObject.GetDataContext<GameState>();
		_animator = GetComponent<Animator>();
	}

	void Update ()
	{
		_gameState.CrashTime = CrashTime;

		if (_gameState.Health <= 0 && _gameState.IsAlive)
		{
			_gameState.IsAlive = false;
			GetComponent<AudioSource>().Play();
			Invoke("SetGameOver", _gameState.CrashTime);
		}

		_animator.SetBool("IsDead", !_gameState.IsAlive);
	}

	void SetGameOver()
	{
		_gameState.GameOver = true;
	}
}