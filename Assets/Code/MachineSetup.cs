using System;
using UnityEngine;

public class MachineSetup : MonoBehaviour
{
    private const float WearMultiplier = 0.02f;
    private const float RepairPerSecond = 0.04f;
    private const float MaxDurability = 1f;
    private const float OutputAdjustPerSecond = 10;
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
        if(_machineType.IsPoweredOn)
        {
            var outputChangeMaxDelta = (OutputAdjustPerSecond * Time.deltaTime);
            _machineType.Output = Mathf.MoveTowards(_machineType.Output, _machineType.RequestedOutput, outputChangeMaxDelta);
        }

        var outputRatio = _machineType.Output/_machineType.MaxOutput;
        _machineType.Durability -= WearMultiplier*outputRatio*outputRatio*Time.deltaTime;

        if (_machineType.IsRepairing)
        {
            if (_machineType.Durability >= MaxDurability)
            {
                _machineType.RepairFinished();
            }
            _machineType.Durability += RepairPerSecond*Time.deltaTime;
        }
    }
}
