using UnityEngine;
using UnityEngine.UI;

public class MachineTypeData : MonoBehaviour
{
    public Text Name;
    public object MachineType { get; private set; }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetMachineType(object machineType)
    {
        var type = (MachineType)machineType;
        Name.text = type.Name;
        MachineType = machineType;
    }
}