using UnityEngine;
using System.Collections;

public class CoalProcess : MonoBehaviour, IMachineProcess
{
	public RectTransform TemperatureBar;
	public float TemperatureBarMaxLength;
	public float TempPerShovel;
	public float NormalTempDecreasePerSecond;
	public float ShutdownTempDecreasePerSecond;
	public float OverloadTempIncreasePerSecond;

	private Coal _coal;
	private ScoreUpdater _scoreUpdater;

	public void Initialize(ScoreUpdater scoreInfo, IMachineType machineType)
	{
		_scoreUpdater = scoreInfo;
		_coal = (Coal)machineType;
		GetComponent<DataContext>().Data = _coal;

		var outputUpdaterComponent = GetComponent<OutputUpdater>();
		outputUpdaterComponent.Initialize(scoreInfo, machineType);
	}

	public void Update()
	{
		if (_coal.IsOverloaded)
		{
			_coal.Temperature = Mathf.Min(_coal.Temperature + OverloadTempIncreasePerSecond*Time.deltaTime, 1f);
		}

		var tempDecreasePerSecond = _coal.IsPoweredOn ? NormalTempDecreasePerSecond : ShutdownTempDecreasePerSecond;
		_coal.Temperature = Mathf.Max(0f, _coal.Temperature - tempDecreasePerSecond*Time.deltaTime);
		_coal.Output = TemperatureToOutput(_coal.Temperature);

		var newBarLength = (_coal.Temperature / _coal.OptimalTempRange.High) * TemperatureBarMaxLength;
		TemperatureBar.sizeDelta = new Vector2(newBarLength, TemperatureBar.sizeDelta.y);
	}

	public void Shovel()
	{
		if(_coal.IsPoweredOn && _scoreUpdater.Income > _coal.ShovelCost)
		{
			_coal.Temperature = Mathf.Min(_coal.Temperature + TempPerShovel, 1.0f);
			_scoreUpdater.Income -= _coal.ShovelCost;
		}
	}

	private float TemperatureToOutput(float temperature)
	{
		if (temperature < _coal.OptimalTempRange.Low)
		{
			return (temperature/_coal.OptimalTempRange.Low)*_coal.MaxOutputPerSecond;
		}

		return _coal.MaxOutputPerSecond;
	}

	public void Overload()
	{
		if (!_coal.IsPoweredOn)
		{
			_coal.TogglePower();
		}
		_coal.IsOverloaded = true;
	}
}