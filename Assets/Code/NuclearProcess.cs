using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NuclearProcess : MonoBehaviour
{
	public RectTransform TemperatureBar;

	// Testing
	public ScoreManager ScoreManager;

	private Nuclear _nuclear;
	private float _newBarHeight;
	private const float MaxTemperature = FuelRod.BaseTemperature*9;
	private const float CooldownPerSecond = 0.5f;
	public const float TemperatureBarMaxHeight = 210;

	private const float MaxRodOutput = 10;
	private const float DegradationPerSecond = 0.01f;
	private const float MaxTemperatureShift = 0.2f;

	public void Initialize(ScoreManager outputManager, IMachineType machineType)
	{
		_nuclear = (Nuclear)machineType;
		GetComponent<DataContext>().Data = _nuclear;

		var outputUpdaterComponent = GetComponent<OutputUpdater>();
		outputUpdaterComponent.Initialize(outputManager, machineType);
	}

	void Update ()
	{
		foreach (var fuelRod in _nuclear.FuelRods)
		{
			if(_nuclear.IsPoweredOn)
			{
				UpdateRodStatus(fuelRod);
			}
		}

		if (_nuclear.IsPoweredOn)
		{
			_nuclear.Output = _nuclear.FuelRods.Sum(fuelRod => fuelRod.Output)*_nuclear.ControlRodDepth;
			_nuclear.Temperature = _nuclear.FuelRods.Sum(fuelRod => fuelRod.Temperature)*_nuclear.ControlRodDepth;
		}
		else
		{
			_nuclear.Output = 0;
			_nuclear.Temperature -= CooldownPerSecond*Time.deltaTime;
		}

		_newBarHeight = (_nuclear.Temperature / MaxTemperature) * TemperatureBarMaxHeight;
		TemperatureBar.sizeDelta = new Vector2(TemperatureBar.sizeDelta.x, _newBarHeight);

		if (_nuclear.Temperature < _nuclear.NoReactionUnit.High*_nuclear.MaxTemperature || _nuclear.Temperature > _nuclear.OverHeatUnit.Low*_nuclear.MaxTemperature)
		{
			_nuclear.PowerOff();
		}
	}

	private static void UpdateRodStatus(FuelRod fuelRod)
	{
		var degradationInverse = 1 - fuelRod.Degradation;
		fuelRod.Degradation += DegradationPerSecond*Time.deltaTime;
		fuelRod.Output = degradationInverse*MaxRodOutput;

		var rodTemperatureShift = MaxTemperatureShift*degradationInverse;
		fuelRod.Temperature = FuelRod.BaseTemperature*degradationInverse + Random.Range(-rodTemperatureShift, rodTemperatureShift);
	}

	public void TogglePower()
	{
		if(_nuclear.IsPoweredOn || _nuclear.Temperature <= 0)
		{
			_nuclear.TogglePower();
		}
	}

	public void SwapRod(int index)
	{
		var fuelRod = new FuelRod();
		_nuclear.FuelRods[index] = fuelRod;
	}
}
