using System;
using UnityEngine;

public class OutputUpdater : MonoBehaviour
{
    public ScoreManager OutputManager { get; set; }
    private IMachineType _machineType;

    public void Initialize(ScoreManager outputManager, IMachineType machineType)
    {
        _machineType = machineType;
        OutputManager = outputManager;
    }

    void Update()
    {
        if (OutputManager != null)
        {
            OutputManager.SetOutput(gameObject, _machineType.Output);
        }
    }
}
