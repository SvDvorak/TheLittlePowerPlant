using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScoreManager
{
	static ScoreManager()
	{
		BuiltMachines = new List<BuiltMachine>();
	}

	public static float CityValue { get; set; }
	public static List<BuiltMachine> BuiltMachines { get; set; }

	public static void MachineBuilt(IMachineType machine, Vector3 position)
	{
		BuiltMachines.Add(new BuiltMachine() { MachineType = machine, Position = position });
	}

	public class BuiltMachine
	{
		public IMachineType MachineType;
		public Vector3 Position;
	}
}

public class ScoreUpdater : MonoBehaviour
{
	public float MaxOutput;
	public float EnergySellPrice;
	public float EnergyBuyCost;
	public float MaxIncomeLoss;
	public float OverloadMaxTime;
	public float OverloadAmount;
	public float FixedIncomePerSecond;
	public float Income;
	public float BaseOutput;
	public float CityGrowthPerSecond;
	public float CityValueToMinimumOutputRatio;

	private bool _gameEnded = false;

	public ForcedOverload ForcedOverload;
	public Animator TransformAnimator;
	public GameObject GameOverScreen;

	private float OverloadPerSecond { get { return 1/OverloadMaxTime;} }
	
	public float CityValue { get { return ScoreManager.CityValue; } }
    public float Output { get; private set; }

	public float MinimumOutputRequired { get { return CityValue * CityValueToMinimumOutputRatio; } }
	public Range CurrentOutputInUnit { get { return new Range(0, Output/MaxOutput); } }
	public Range MinimumOutputInUnit { get { return new Range(0, MinimumOutputRequired / MaxOutput); } }
	public Range OverloadOutputInUnit { get { return new Range(OutputOverloadLimit / MaxOutput, 1.0f); } }
	public float OutputOverloadLimit { get { return MaxOutput*0.8f; } }

	private readonly Dictionary<object, float> _machineOutputs = new Dictionary<object, float>();
	private readonly Dictionary<object, float> _machineMaxOutputs = new Dictionary<object, float>();

	public ScoreUpdater()
    {
        ScoreManager.CityValue = 15;
        Output = 0;
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
		if(!_gameEnded)
		{
			UpdateCityValue();
			UpdateOutput();
			UpdateIncome();
			CheckOverloading();
			CheckBroke();
		}
    }

	private void UpdateCityValue()
	{
		ScoreManager.CityValue += CityGrowthPerSecond*Time.deltaTime;
	}

	private void UpdateOutput()
	{
		Output = Mathf.Min(_machineOutputs.Values.Sum(), MaxOutput) + BaseOutput;
		var maxMachineOutputs = _machineMaxOutputs.Count == 0 ? BaseOutput : _machineMaxOutputs.Values.Sum();
		MaxOutput = maxMachineOutputs + BaseOutput;
	}

	private void UpdateIncome()
	{
		var outputDiff = Output - MinimumOutputRequired;
		var incomeChangeMultiplier = outputDiff < 0 ? EnergyBuyCost : EnergySellPrice;

		Income += FixedIncomePerSecond*Time.deltaTime + outputDiff*incomeChangeMultiplier*Time.deltaTime;
	}

	private void CheckOverloading()
	{
		if (Output > OutputOverloadLimit)
		{
			var amountOverLimitInUnit = (Output - OutputOverloadLimit)/(MaxOutput - OutputOverloadLimit);
			OverloadAmount += OverloadPerSecond*amountOverLimitInUnit*Time.deltaTime;

			if (OverloadAmount > 1)
			{
				_gameEnded = true;
				//GameOverScreen.SetActive(true);
				TransformAnimator.SetBool("IsTransformed", true);
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