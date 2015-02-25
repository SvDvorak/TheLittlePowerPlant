using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ScoreManager
{
	public static float CityValue { get; set; }
}

public class ScoreUpdater : MonoBehaviour
{
	public float MaxOutput = 400f;
	public float EnergySellPrice = 1f;
	public float EnergyBuyCost = 2f;
	public float OutputOverloadLimit;
	public float OverloadMaxTime = 3f;
	public float OverloadAmount;

	private float OverloadPerSecond { get { return 1/OverloadMaxTime;} }

	public float CityValue { get { return ScoreManager.CityValue; } }
    public float Output { get; private set; }
    public float Income { get; set; }

	public float MinimumOutputRequired { get { return CityValue / 100f; } }
	public Range CurrentOutputInUnit { get { return new Range(0, Output/MaxOutput); } }
	public Range MinimumOutputInUnit { get { return new Range(0, MinimumOutputRequired / MaxOutput); } }
	public Range OverloadOutputInUnit { get { return new Range(OutputOverloadLimit / MaxOutput, 1.0f); } }

	private readonly Dictionary<object, float> _machineOutputs = new Dictionary<object, float>();

    public ScoreUpdater()
    {
        ScoreManager.CityValue = 5000;
        Output = 0;
        Income = 250000;
	    OutputOverloadLimit = MaxOutput*0.8f;
	}

    public void SetOutput(object machine, float output)
    {
        if (!_machineOutputs.ContainsKey(machine))
        {
            _machineOutputs.Add(machine, output);
        }
        else
        {
            _machineOutputs[machine] = output;
        }
    }

    public void Update()
    {
		UpdateCityValue();
		UpdateOutput();
	    UpdateIncome();
	    CheckOverloading();
    }

	private static void UpdateCityValue()
	{
		ScoreManager.CityValue += (69 + Random.Range(-3, 3))*Time.deltaTime;
	}

	private void UpdateOutput()
	{
		Output = Mathf.Min(_machineOutputs.Values.Sum(), MaxOutput);
	}

	private void UpdateIncome()
	{
		var outputDiff = Output - MinimumOutputRequired;
		var incomeChangeMultiplier = EnergySellPrice;
		if (outputDiff < 0)
		{
			incomeChangeMultiplier = EnergyBuyCost;
		}

		Income += outputDiff*incomeChangeMultiplier*Time.deltaTime;
	}

	private void CheckOverloading()
	{
		if (Output > OutputOverloadLimit)
		{
			var amountOverLimitInUnit = (Output - OutputOverloadLimit)/(MaxOutput - OutputOverloadLimit);
			OverloadAmount += OverloadPerSecond*amountOverLimitInUnit*Time.deltaTime;

			if (OverloadAmount > 1)
			{
				Application.LoadLevel("Part2");
			}
		}
		else
		{
			OverloadAmount = 0;
		}
	}
}