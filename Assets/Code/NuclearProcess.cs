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
	private const float MaxTemperature = FuelRodProcess.RodBaseTemperature*9;
	private const float TemperatureBarMaxHeight = 210;

	public void Awake()
	{
		Initialize(ScoreManager, new Nuclear());
	}

	public void Initialize(ScoreManager outputManager, IMachineType machineType)
	{
		_nuclear = (Nuclear)machineType;
		GetComponent<DataContext>().Data = _nuclear;

		var outputUpdaterComponent = GetComponent<OutputUpdater>();
		outputUpdaterComponent.Initialize(outputManager, machineType);
	}

	void Update ()
	{
		_nuclear.Output = _nuclear.FuelRods.Sum(fuelRod => fuelRod.Output)*_nuclear.ControlRodDepth;
		_nuclear.Temperature = _nuclear.FuelRods.Sum(fuelRod => fuelRod.Temperature)*_nuclear.ControlRodDepth;

		_newBarHeight = (_nuclear.Temperature/MaxTemperature)*TemperatureBarMaxHeight;
		TemperatureBar.sizeDelta = new Vector2(TemperatureBar.sizeDelta.x, _newBarHeight);
	}
}
