using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{
    public Text OutputVelocity;
	public ScoreUpdater ScoreUpdater;
	private float _previousIncome;

	void Update ()
	{
		OutputVelocity.text = "IncomeVelocity: " + (ScoreUpdater.Income - _previousIncome);
		_previousIncome = ScoreUpdater.Income;
	}
}
