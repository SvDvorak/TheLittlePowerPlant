using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public float MaxOutput { get { return 400; }}

    public float CityValue { get; private set; }
    public float Output { get; private set; }
    public float Income { get; private set; }

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
        CityValue += (69 + Random.Range(-3, 3))*Time.deltaTime;
        Output = _machineOutputs.Values.Sum();
        Income += (Output - MinimumOutputRequired)*Time.deltaTime;
    }
}