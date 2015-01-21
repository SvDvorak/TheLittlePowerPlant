using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public float MaxOutput { get { return 400; }}

    public float CityValue { get; private set; }
    public float Output { get; private set; }

    private readonly Dictionary<object, float> _machineOutputs = new Dictionary<object, float>();

    public ScoreManager()
    {
        CityValue = 0;
        Output = 0;
    }

    public float MinimumOutputRequired
    {
        get { return CityValue/100f; }
    }

    public void SetOutput(object machine, float output)
    {
        if (!_machineOutputs.ContainsKey(machine))
        {
            _machineOutputs.Add(machine, output);
        }
        else
        {
            _machineOutputs[machine] = output;
        }
    }

    public void Update()
    {
        CityValue += (969 + Random.Range(-3, 3))*Time.deltaTime;
        Output += (5 +Random.Range(-5, 6))*Time.deltaTime;
    }
}