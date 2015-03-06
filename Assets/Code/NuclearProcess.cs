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
	public float ControlRodAlterSpeed = 0.0001f;

	public float DegradationPerSecond = 0.01f;
	public float MaxTemperatureShift = 0.2f;
	public double MaxTimeOutsideLimit = 3;
	public double MaxOutsideLimitTimePerSecond { get { return 1/MaxTimeOutsideLimit; } }
	private double _outsideLimitAccumulated;

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

		_nuclear.ControlRodEffect = Mathf.MoveTowards(_nuclear.ControlRodEffect, _nuclear.ControlRodDepth, ControlRodAlterSpeed*Time.deltaTime);

		if (_nuclear.IsPoweredOn)
		{
			_nuclear.Output = _nuclear.FuelRods.Sum(fuelRod => fuelRod.Output)*_nuclear.ControlRodEffect;
			_nuclear.Temperature = Mathf.Min(_nuclear.FuelRods.Sum(fuelRod => fuelRod.Temperature)*_nuclear.ControlRodEffect, MaxTemperature);
		}
		else
		{
			_nuclear.Output = 0;
			_nuclear.Temperature -= CooldownPerSecond*Time.deltaTime;
		}

		UpdateTimeOutsideLimits();

		if (_outsideLimitAccumulated > 1 && !_nuclear.IsOverloaded)
		{
			_nuclear.PowerOff();
		}
	}

	private void UpdateTimeOutsideLimits()
	{
		var temperatureUnit = _nuclear.Temperature/_nuclear.MaxTemperature;
		var outsideLimitAmount = 0f;

		if (temperatureUnit < _nuclear.NoReactionUnit.High)
		{
			outsideLimitAmount = 1 -
			                     (temperatureUnit - _nuclear.NoReactionUnit.Low)/
			                     (_nuclear.NoReactionUnit.High - _nuclear.NoReactionUnit.Low);
		}
		else if (temperatureUnit > _nuclear.OverHeatUnit.Low)
		{
			outsideLimitAmount = (temperatureUnit - _nuclear.OverHeatUnit.Low)/
			                     (_nuclear.OverHeatUnit.High - _nuclear.OverHeatUnit.Low);
		}
		else
		{
			_outsideLimitAccumulated = 0f;
		}

		_outsideLimitAccumulated += outsideLimitAmount*MaxOutsideLimitTimePerSecond*Time.deltaTime;
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
