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
	public float EnergySellPrice = 1f;
	public float EnergyBuyCost = 2f;
	public float OutputOverload;
	public float MaxOutput = 400f;

    public float CityValue { get { return ScoreManager.CityValue; } }
    public float Output { get; private set; }
    public float Income { get; set; }

    private readonly Dictionary<object, float> _machineOutputs = new Dictionary<object, float>();

    public ScoreUpdater()
    {
        ScoreManager.CityValue = 5000;
        Output = 0;
        Income = 250000;
	    OutputOverload = MaxOutput*0.8f;
    }

    public float MinimumOutputRequired
    {
        get { return CityValue/100f; }
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
        ScoreManager.CityValue += (69 + Random.Range(-3, 3))*Time.deltaTime;
        Output = _machineOutputs.Values.Sum();

        var outputDiff = Output - MinimumOutputRequired;
        var incomeChangeMultiplier = EnergySellPrice;
        if (outputDiff < 0)
        {
            incomeChangeMultiplier = EnergyBuyCost;
        }
        Income += outputDiff*incomeChangeMultiplier*Time.deltaTime;

	    if (Output > OutputOverload)
	    {
		    Application.LoadLevel("Part2");
	    }
    }
}