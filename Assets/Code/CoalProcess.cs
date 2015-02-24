using UnityEngine;
using System.Collections;

public class CoalProcess : MonoBehaviour
{
	public RectTransform TemperatureBar;
	public float OutputMax = 100f;
	public float TemperatureBarMaxLength = 100f;
	public float TempPerShovel = 0.01f;
	public float NormalTempDecreasePerSecond = 0.01f;
	public float ShutdownTempDecreasePerSecond = 0.005f;

	private Coal _coal;

	public void Initialize(ScoreManager outputManager, IMachineType machineType)
	{
		_coal = (Coal)machineType;
		GetComponent<DataContext>().Data = _coal;

		var outputUpdaterComponent = GetComponent<OutputUpdater>();
		outputUpdaterComponent.Initialize(outputManager, machineType);
	}

	public void Update()
	{
		var tempDecreasePerSecond = _coal.IsPoweredOn ? NormalTempDecreasePerSecond : ShutdownTempDecreasePerSecond;
		_coal.Temperature = Mathf.Max(0f, _coal.Temperature - tempDecreasePerSecond*Time.deltaTime);

		_coal.Output = TemperatureToOutput(_coal.Temperature);

		var newBarLength = (_coal.Temperature / _coal.OptimalTempRange.High) * TemperatureBarMaxLength;
		TemperatureBar.sizeDelta = new Vector2(newBarLength, TemperatureBar.sizeDelta.y);
	}

	public void Shovel()
	{
		if(_coal.IsPoweredOn)
		{
			_coal.Temperature = Mathf.Min(_coal.Temperature + TempPerShovel, 1.0f);
		}
	}

	private float TemperatureToOutput(float temperature)
	{
		if (temperature < _coal.OptimalTempRange.Low)
		{
			return (temperature/_coal.OptimalTempRange.Low)*OutputMax;
		}

		return OutputMax;
	}
}
