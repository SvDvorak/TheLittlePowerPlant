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
		InvokeRepeating("UpdateCityValue", 0, 1);
		InvokeRepeating("UpdateIncome", 1, 1);
	}

	public void UpdateCityValue()
	{
		CityValueText.text = "City value: " + (int)ScoreUpdater.CityValue + "M$";
		MinOutputText.text = "Min output: " + (int)ScoreUpdater.MinimumOutputRequired + "MWh";
	}

	public void UpdateIncome()
	{
	    IncomeText.text = "Income: " + (int) ScoreUpdater.Income + "K$";
	}
}
