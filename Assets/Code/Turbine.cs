using UnityEngine;

public interface IMachineType
{
    string Name { get; set; }
    bool IsPoweredOn { get; }
    float Output { get; set; }
    float MinOutput { get; }
    float MaxOutput { get; }

    void TogglePower();
}

public class Turbine : IMachineType
{
    public string Name { get; set; }
    public float Output { get; set; }
    public float RequestedOutput { get; set; }
    public bool IsPoweredOn { get; private set; }
    public float Durability { get; set; }

    public float MinOutput { get { return 50; } }
    public float MaxOutput { get { return 150; } }

    public Turbine()
    {
        IsPoweredOn = true;
        Output = MinOutput;
        Durability = 1;
    }

    public void TogglePower()
    {
        IsPoweredOn = !IsPoweredOn;
        Output = IsPoweredOn ? MinOutput : 0;
    }
}