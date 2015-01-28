using Assets.Code;
using UnityEngine;
using UnityEngine.UI;

public class SingleMachineOutputBar : MonoBehaviour
{
    public Slider SelectedOutput;
    public RectTransform ActualOutput;

    public float OutputAdjustPerSecond = 5;
    private IMachineType _machineType;

    void Start ()
    {
        _machineType = (IMachineType)gameObject.GetDataContext();
    }

    void Update ()
	{
        ActualOutput.localScale = new Vector3(Mathf.Max(GetOutputInUnitInterval(), 0.02f), 1, 1);
	}

    private float GetOutputInUnitInterval()
    {
        return (_machineType.Output - _machineType.MinOutput)/(_machineType.MaxOutput - _machineType.MinOutput);
    }
}
