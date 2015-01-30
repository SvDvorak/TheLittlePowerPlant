using System;
using UnityEngine;

public class MachineSetup : MonoBehaviour
{
    private const float WearMultiplier = 0.02f;
    private const float RepairPerSecond = 0.04f;
    private const float MaxDurability = 1f;
    private const float BreakdownSafeZone = 0.3f;
    private const float OutputAdjustPerSecond = 10;
    private Turbine _machineType;

    public void Initialize(ScoreManager outputManager, IMachineType machineType)
    {
        _machineType = (Turbine)machineType;
        var outputUpdaterComponent = GetComponent<OutputUpdater>();
        outputUpdaterComponent.Initialize(outputManager, machineType);

        GetComponentInChildren<DataContext>().Data = machineType;
        InvokeRepeating("PerformBreakCheck", 0, 1);
    }

    public void Update()
    {
        CalculateOutput();
        CalculateWear();
        CalculateRepair();
    }

    private void CalculateOutput()
    {
        if (_machineType.IsPoweredOn)
        {
            var outputChangeMaxDelta = (OutputAdjustPerSecond*Time.deltaTime);
            _machineType.Output = Mathf.MoveTowards(_machineType.Output, _machineType.RequestedOutput, outputChangeMaxDelta);
        }
    }

    private void CalculateWear()
    {
        var outputRatio = _machineType.Output/_machineType.MaxOutput;
        _machineType.Durability -= WearMultiplier*outputRatio*outputRatio*Time.deltaTime;
    }

    private void CalculateRepair()
    {
        if (_machineType.IsRepairing)
        {
            if (_machineType.Durability >= MaxDurability)
            {
                _machineType.RepairFinished();
            }
            _machineType.Durability += RepairPerSecond*Time.deltaTime;
        }
    }

    private void PerformBreakCheck()
    {
        var randomUnitInterval = UnityEngine.Random.Range(0, 1f);
        if (Math.Pow(randomUnitInterval, 10) - BreakdownSafeZone > _machineType.Durability)
        {
            _machineType.Break();
        }
    }
}
