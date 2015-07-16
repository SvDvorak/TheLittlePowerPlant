using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreText : MonoBehaviour
{
	private Text _text;

	void Start()
	{
		_text = GetComponent<Text>();
	}

	void Update ()
	{
		_text.text = string.Format("Score: {0}K$", ScoreManager.CityValue);
	}
}
