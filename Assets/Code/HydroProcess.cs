using System;
using UnityEngine;

public class HydroProcess : MonoBehaviour, IMachineProcess
{
    private const float WearMultiplier = 0.02f;
    private const float RepairPerSecond = 0.025f;
    private const float MaxDurability = 1f;
    private const float FlowAdjustPerSecond = 10;
	private const float FlowToOutputRatio = 1f;
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
        UpdateFlow();
	    UpdateOutput();
        CalculateWear();
        CalculateRepair();
    }

	private void UpdateFlow()
    {
        if (_hydro.IsPoweredOn)
        {
            var flowChangeMaxDelta = (FlowAdjustPerSecond*Time.deltaTime);
            _hydro.CurrentFlow = Mathf.MoveTowards(_hydro.CurrentFlow, _hydro.RequestedFlow, flowChangeMaxDelta);
        }
    }

	private void UpdateOutput()
	{
		_hydro.Output = _hydro.CurrentFlow*FlowToOutputRatio;
	}

	private void CalculateWear()
    {
        if(_hydro.IsPoweredOn)
        {
            var flowRatio = WearCurve(_hydro.CurrentFlow);
            _hydro.Durability -= Mathf.Max(0f, WearMultiplier*flowRatio*Time.deltaTime);
        }
    }

    // Flow 50-100: 0-0.5
    // Flow 100-120: 0.5-1
    private float WearCurve(float flow)
    {
        if (flow < _hydro.MaxNormalFlow)
        {
            return (flow - _hydro.MinFlow)/_hydro.MaxNormalFlow;
        }

        return 0.5f + (flow - _hydro.MaxNormalFlow)/((_hydro.OverloadFlow - _hydro.MaxNormalFlow)*2);
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
		_hydro.RequestedFlow = _hydro.OverloadFlow;
		if (!_hydro.IsPoweredOn)
		{
			_hydro.TogglePower();
		}
		_hydro.IsOverloaded = true;
	}
}
