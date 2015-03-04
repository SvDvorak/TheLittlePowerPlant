using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NuclearProcess : MonoBehaviour, IMachineProcess
{
	public RectTransform TemperatureBar;
	public float TemperatureBarMaxHeight = 210;
	public float OverloadDegradationDecreasePerSecond;

	private Nuclear _nuclear;
	private const float MaxTemperature = FuelRod.BaseTemperature*9;
	public float CooldownPerSecond = 0.5f;

	public float DegradationPerSecond = 0.01f;
	public float MaxTemperatureShift = 0.2f;

	public void Initialize(ScoreUpdater outputUpdater, IMachineType machineType)
	{
		_nuclear = (Nuclear)machineType;
		GetComponent<DataContext>().Data = _nuclear;

		var outputUpdaterComponent = GetComponent<OutputUpdater>();
		outputUpdaterComponent.Initialize(outputUpdater, machineType);
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
			_nuclear.Temperature = Mathf.Min(_nuclear.FuelRods.Sum(fuelRod => fuelRod.Temperature)*_nuclear.ControlRodDepth, MaxTemperature);
		}
		else
		{
			_nuclear.Output = 0;
			_nuclear.Temperature -= CooldownPerSecond*Time.deltaTime;
		}

		//var newBarHeight = (_nuclear.Temperature / MaxTemperature) * TemperatureBarMaxHeight;
		//TemperatureBar.sizeDelta = new Vector2(TemperatureBar.sizeDelta.x, newBarHeight);

		var isOutsideOperationTemperature = 
			_nuclear.Temperature < _nuclear.NoReactionUnit.High*_nuclear.MaxTemperature ||
			_nuclear.Temperature > _nuclear.OverHeatUnit.Low*_nuclear.MaxTemperature;
		if (isOutsideOperationTemperature && !_nuclear.IsOverloaded)
		{
			_nuclear.PowerOff();
		}
	}

	private void UpdateRodStatus(FuelRod fuelRod)
	{
		fuelRod.Degradation = fuelRod.Degradation + (!_nuclear.IsOverloaded
			? DegradationPerSecond*Time.deltaTime
			: -OverloadDegradationDecreasePerSecond*Time.deltaTime);

		var degradationInverse = Mathf.Clamp(1 - fuelRod.Degradation, 0f, 1f);
		fuelRod.Output = degradationInverse * FuelRod.MaxRodOutput;

		var rodTemperatureShift = MaxTemperatureShift * degradationInverse;
		fuelRod.Temperature = FuelRod.BaseTemperature * degradationInverse + Random.Range(-rodTemperatureShift, rodTemperatureShift);
	}

	public void TogglePower()
	{
		if(!_nuclear.IsOverloaded && (_nuclear.IsPoweredOn || _nuclear.Temperature <= 0))
		{
			_nuclear.TogglePower();
		}
	}

	public void SwapRod(int index)
	{
		var fuelRod = new FuelRod();
		_nuclear.FuelRods[index] = fuelRod;
	}

	public void Overload()
	{
		if (!_nuclear.IsPoweredOn)
		{
			_nuclear.TogglePower();
		}
		_nuclear.IsOverloaded = true;
		_nuclear.ControlRodDepth = 1f;
	}
}
