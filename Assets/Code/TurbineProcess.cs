using System;
using UnityEngine;

public class TurbineProcess : MonoBehaviour, IMachineProcess
{
    private const float WearMultiplier = 0.02f;
    private const float RepairPerSecond = 0.025f;
    private const float MaxDurability = 1f;
    private const float OutputAdjustPerSecond = 10;
    private Turbine _turbine;

    public void Initialize(ScoreUpdater outputUpdater, IMachineType machineType)
    {
        _turbine = (Turbine)machineType;
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
        if (_turbine.IsPoweredOn)
        {
            var outputChangeMaxDelta = (OutputAdjustPerSecond*Time.deltaTime);
            _turbine.Output = Mathf.MoveTowards(_turbine.Output, _turbine.RequestedOutput, outputChangeMaxDelta);
        }
    }

    private void CalculateWear()
    {
        if(_turbine.IsPoweredOn)
        {
            var outputRatio = WearCurve(_turbine.Output);
            _turbine.Durability -= Mathf.Max(0f, WearMultiplier*outputRatio*Time.deltaTime);
        }
    }

    // Output 50-100: 0-0.5
    // Output 100-120: 0.5-1
    private float WearCurve(float output)
    {
        if (output < _turbine.MaxNormalOutput)
        {
            return (output - _turbine.MinOutput)/_turbine.MaxNormalOutput;
        }

        return 0.5f + (output - _turbine.MaxNormalOutput)/((_turbine.OverloadOutput - _turbine.MaxNormalOutput)*2);
    }

    private void CalculateRepair()
    {
        if (_turbine.IsRepairing)
        {
            if (_turbine.Durability >= MaxDurability)
            {
                _turbine.RepairFinished();
            }
            _turbine.Durability += RepairPerSecond*Time.deltaTime;
        }
    }

    private void PerformBreakCheck()
    {
        var randomBreakChance = Math.Pow(UnityEngine.Random.Range(0, 1f), 10);
        if (!_turbine.IsOverloaded && _turbine.IsPoweredOn && randomBreakChance > _turbine.Durability)
        {
            _turbine.Break();
        }
    }

	public void Overload()
	{
		if(!_turbine.IsPoweredOn)
		{
			_turbine.TogglePower();
		}
		_turbine.IsOverloaded = true;
		_turbine.RequestedOutput = _turbine.OverloadOutput;
	}
}
