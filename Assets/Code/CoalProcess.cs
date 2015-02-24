using UnityEngine;
using System.Collections;

public class CoalProcess : MonoBehaviour
{
	private Coal _coal;

	public void Initialize(ScoreManager outputManager, IMachineType machineType)
	{
		_coal = (Coal)machineType;
		GetComponent<DataContext>().Data = _coal;

		var outputUpdaterComponent = GetComponent<OutputUpdater>();
		outputUpdaterComponent.Initialize(outputManager, machineType);
	}

	void Update ()
	{
	
	}
}
