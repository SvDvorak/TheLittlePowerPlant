using System;
using UnityEngine;

public class MachineSetup : MonoBehaviour
{
    public float OutputAdjustPerSecond = 5;
    private Turbine _machineType;

    public void Initialize(ScoreManager outputManager, IMachineType machineType)
    {
        _machineType = (Turbine)machineType;
        var outputUpdaterComponent = GetComponent<OutputUpdater>();
        outputUpdaterComponent.Initialize(outputManager, machineType);

        GetComponentInChildren<DataContext>().Data = machineType;
    }

    public void Update()
    {
        var outputChangeMaxDelta = (OutputAdjustPerSecond * Time.deltaTime);
        _machineType.Output = Mathf.MoveTowards(_machineType.Output, _machineType.RequestedOutput, outputChangeMaxDelta);
    }
}
