using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreLoader : MonoBehaviour
{
	public void OnEnable()
	{
		var readNames = PlayerPrefs2.GetStringArray("Names");
		foreach (var name in readNames)
		{
			Debug.Log(name);
		}
	}
}