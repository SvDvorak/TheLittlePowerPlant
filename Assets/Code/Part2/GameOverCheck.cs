using UnityEngine;
using System.Collections;
using System.Linq;

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
			var readNames = PlayerPrefs2.GetStringArray("Names").ToList();
			var readScores = PlayerPrefs2.GetFloatArray("Scores").ToList();
			readNames.Add("Andreas");
			readScores.Add(ScoreManager.DestructionValue);
			PlayerPrefs2.SetStringArray("Names", readNames.ToArray());
			PlayerPrefs2.SetFloatArray("Scores", readScores.ToArray());
			HighscoreElement.SetActive(true);
		}
	}
}