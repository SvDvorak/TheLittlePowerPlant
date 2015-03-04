using Assets.Code;
using UnityEngine;
using UnityEngine.UI;

public class SingleMachineOutputBar : MonoBehaviour
{
    public RectTransform ActualOutput;
    private Hydro _machineType;

    void Start ()
    {
        _machineType = gameObject.GetDataContext<Hydro>();
    }

    void Update ()
	{
        ActualOutput.localScale = new Vector3(Mathf.Max(GetOutputInUnitInterval(), 0.02f), 1, 1);
	}

    public float GetOutputInUnitInterval()
    {
        return (_machineType.Output - _machineType.MinOutput) / (_machineType.OverloadOutput - _machineType.MinOutput);
    }
}
