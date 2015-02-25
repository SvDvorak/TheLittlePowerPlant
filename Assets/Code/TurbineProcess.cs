using System;
using UnityEngine;

public class TurbineProcess : MonoBehaviour
{
    private const float WearMultiplier = 0.02f;
    private const float RepairPerSecond = 0.025f;
    private const float MaxDurability = 1f;
    private const float OutputAdjustPerSecond = 10;
    private Turbine _machineType;

    public void Initialize(ScoreUpdater outputUpdater, IMachineType machineType)
    {
        _machineType = (Turbine)machineType;
        var outputUpdaterComponent = GetComponent<OutputUpdater>();
        outputUpdaterComponent.Initialize(outputUpdater, machineType);

	    GetComponent<DataContext>().Data = machineType;
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
        if(_machineType.IsPoweredOn)
        {
            var outputRatio = WearCurve(_machineType.Output);
            _machineType.Durability -= WearMultiplier*outputRatio*Time.deltaTime;
        }
    }

    // Output 50-100: 0-0.5
    // Output 100-120: 0.5-1
    private float WearCurve(float output)
    {
        if (output < _machineType.MaxNormalOutput)
        {
            return (output - _machineType.MinOutput)/_machineType.MaxNormalOutput;
        }

        return 0.5f + (output - _machineType.MaxNormalOutput)/((_machineType.OverloadOutput - _machineType.MaxNormalOutput)*2);
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
        var randomBreakChance = Math.Pow(UnityEngine.Random.Range(0, 1f), 10);
        if (_machineType.IsPoweredOn && randomBreakChance > _machineType.Durability)
        {
            _machineType.Break();
        }
    }
}
