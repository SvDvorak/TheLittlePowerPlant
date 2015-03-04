using UnityEngine;
using System.Collections;
using Assets.Code;
using UnityEngine.UI;

public class CurrentOutput : MonoBehaviour
{
    public ScoreUpdater ScoreUpdater;
    public Text CityValueText;
    public Text MinOutputText;
    public Text IncomeText;

	public void Awake()
	{
		GetComponent<DataContext>().Data = ScoreUpdater;
	}

	void Update ()
	{
		CityValueText.text = "City value: " + (int)ScoreUpdater.CityValue + "$";
        MinOutputText.text = "Min output: " + (int)ScoreUpdater.MinimumOutputRequired + "kw";
	    IncomeText.text = "Income: " + (int) ScoreUpdater.Income + "$";
	}
}
