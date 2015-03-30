using UnityEngine;
using System.Collections;

public class GameOverCheck : MonoBehaviour
{
	public DataContext GamestateContext;
	public GameObject HighscoreElement;
	private GameState _gameState;

	void Start()
	{
		_gameState = GamestateContext.Data as GameState;
	}

	void Update ()
	{
		if (_gameState.GameOver && !HighscoreElement.activeSelf)
		{
			HighscoreElement.SetActive(true);
		}
	}
}