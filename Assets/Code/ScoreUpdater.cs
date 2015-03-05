using System;
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
	public float MaxOutput;
	public float EnergySellPrice = 1f;
	public float EnergyBuyCost = 2f;
	public float MaxIncomeLoss;
	public float OverloadMaxTime = 3f;
	public float OverloadAmount;
	public float Income;
	public float BaseOutput;
	public float CityGrowthPerSecond = 69;

	public ForcedOverload ForcedOverload;

	private float OverloadPerSecond { get { return 1/OverloadMaxTime;} }
	
	public float CityValue { get { return ScoreManager.CityValue; } }
    public float Output { get; private set; }

	public float MinimumOutputRequired { get { return CityValue / 100f; } }
	public Range CurrentOutputInUnit { get { return new Range(0, Output/MaxOutput); } }
	public Range MinimumOutputInUnit { get { return new Range(0, MinimumOutputRequired / MaxOutput); } }
	public Range OverloadOutputInUnit { get { return new Range(OutputOverloadLimit / MaxOutput, 1.0f); } }
	public float OutputOverloadLimit { get { return MaxOutput*0.8f; } }

	private readonly Dictionary<object, float> _machineOutputs = new Dictionary<object, float>();
	private readonly Dictionary<object, float> _machineMaxOutputs = new Dictionary<object, float>();

	public ScoreUpdater()
    {
        ScoreManager.CityValue = 5000;
        Output = 0;
        Income = 500000;
	}

    public void SetOutput(IMachineType machine)
    {
        if (!_machineOutputs.ContainsKey(machine))
        {
            _machineOutputs.Add(machine, machine.Output);
	        _machineMaxOutputs.Add(machine, machine.MaxOutputPerSecond);
        }
        else
        {
            _machineOutputs[machine] = machine.Output;
	        _machineMaxOutputs[machine] = machine.MaxOutputPerSecond;
        }
	}

    public void Update()
    {
		UpdateCityValue();
		UpdateOutput();
	    UpdateIncome();
	    CheckOverloading();
	    CheckBroke();
    }

	private void UpdateCityValue()
	{
		ScoreManager.CityValue += CityGrowthPerSecond*Time.deltaTime;
	}

	private void UpdateOutput()
	{
		Output = Mathf.Min(_machineOutputs.Values.Sum(), MaxOutput) + BaseOutput;
		MaxOutput = _machineMaxOutputs.Count == 0 ? 100 : _machineMaxOutputs.Values.Sum();
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

	private void CheckBroke()
	{
		if (!ForcedOverload.Overloaded && MaxIncomeLoss > Income && (Output - MinimumOutputRequired) < 0)
		{
			ForcedOverload.ForceOverload();
		}
	}
}