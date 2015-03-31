using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScoreLoader : MonoBehaviour
{
	public ItemList ItemList;

	public void OnEnable()
	{
		var readNames = PlayerPrefs2.GetStringArray("Names");
		var readScores = PlayerPrefs2.GetFloatArray("Scores");

		var scoreEntries = new List<HighscoreEntry>();
		for (int i = 0; i < readNames.Count(); i++)
		{
			scoreEntries.Add(new HighscoreEntry(readNames[i], readScores[i]));
		}

		ItemList.Items = scoreEntries.OrderByDescending(x => x.Score).Cast<object>();
	}
}

public class HighscoreEntry
{
	public HighscoreEntry(string name, float score)
	{
		Name = name;
		Score = score;
	}

	public string Name { get; private set; }
	public float Score { get; private set; }
}