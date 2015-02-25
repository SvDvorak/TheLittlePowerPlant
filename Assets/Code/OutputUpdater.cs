using System;
using UnityEngine;

public class OutputUpdater : MonoBehaviour
{
    public ScoreUpdater ScoreUpdater { get; set; }
    private IMachineType _machineType;

    public void Initialize(ScoreUpdater scoreUpdater, IMachineType machineType)
    {
        _machineType = machineType;
        ScoreUpdater = scoreUpdater;
    }

    void Update()
    {
        if (ScoreUpdater != null)
        {
            ScoreUpdater.SetOutput(gameObject, _machineType.Output);
        }
    }
}
