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

	public void Initialize(ScoreManager outputManager, IMachineType machineType)
	{
		_nuclear = (Nuclear)machineType;
		GetComponent<DataContext>().Data = _nuclear;

		var outputUpdaterComponent = GetComponent<OutputUpdater>();
		outputUpdaterComponent.Initialize(outputManager, machineType);
	}

	void Update ()
	{
		if(_nuclear.IsPoweredOn)
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

	public void TogglePower()
	{
		if(_nuclear.IsPoweredOn || _nuclear.Temperature <= 0)
		{
			_nuclear.TogglePower();
		}
	}
}
