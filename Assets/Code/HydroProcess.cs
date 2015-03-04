using System;
using UnityEngine;

public class HydroProcess : MonoBehaviour, IMachineProcess
{
    private const float WearMultiplier = 0.02f;
    private const float RepairPerSecond = 0.025f;
    private const float MaxDurability = 1f;
    private const float OutputAdjustPerSecond = 10;
    private Hydro _hydro;

    public void Initialize(ScoreUpdater outputUpdater, IMachineType machineType)
    {
        _hydro = (Hydro)machineType;
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
        if (_hydro.IsPoweredOn)
        {
            var outputChangeMaxDelta = (OutputAdjustPerSecond*Time.deltaTime);
            _hydro.Output = Mathf.MoveTowards(_hydro.Output, _hydro.RequestedOutput, outputChangeMaxDelta);
        }
    }

    private void CalculateWear()
    {
        if(_hydro.IsPoweredOn)
        {
            var outputRatio = WearCurve(_hydro.Output);
            _hydro.Durability -= Mathf.Max(0f, WearMultiplier*outputRatio*Time.deltaTime);
        }
    }

    // Output 50-100: 0-0.5
    // Output 100-120: 0.5-1
    private float WearCurve(float output)
    {
        if (output < _hydro.MaxNormalOutput)
        {
            return (output - _hydro.MinOutput)/_hydro.MaxNormalOutput;
        }

        return 0.5f + (output - _hydro.MaxNormalOutput)/((_hydro.OverloadOutput - _hydro.MaxNormalOutput)*2);
    }

    private void CalculateRepair()
    {
        if (_hydro.IsRepairing)
        {
            if (_hydro.Durability >= MaxDurability)
            {
                _hydro.RepairFinished();
            }
            _hydro.Durability += RepairPerSecond*Time.deltaTime;
        }
    }

    private void PerformBreakCheck()
    {
        var randomBreakChance = Math.Pow(UnityEngine.Random.Range(0, 1f), 10);
        if (!_hydro.IsOverloaded && _hydro.IsPoweredOn && randomBreakChance > _hydro.Durability)
        {
            _hydro.Break();
        }
    }

	public void Overload()
	{
		_hydro.IsBroke = false;
		_hydro.RequestedOutput = _hydro.OverloadOutput;
		if (!_hydro.IsPoweredOn)
		{
			_hydro.TogglePower();
		}
		_hydro.IsOverloaded = true;
	}
}
