using UnityEngine;

public interface IMachineType
{
    string Name { get; set; }
    int Cost { get; set; }
    bool IsPoweredOn { get; }
    float Output { get; set; }
    float MinOutput { get; }
    float MaxOutput { get; }

    void TogglePower();
    void Repair();
}

public class Turbine : IMachineType
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public float Output { get; set; }
    public float RequestedOutput { get; set; }
    public bool IsPoweredOn { get; private set; }
    public bool IsRepairing { get; private set; }
    public bool IsBroke { get; private set; }
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
        if (IsPoweredOn)
        {
            PowerOff();
        }
        else
        {
            PowerOn();
        }
    }

    private void PowerOn()
    {
        if(!IsBroke)
        {
            IsPoweredOn = true;
            Output = MinOutput;
        }
    }

    private void PowerOff()
    {
        IsPoweredOn = false;
        Output = 0;
    }

    public void Repair()
    {
        IsRepairing = true;
        PowerOff();
    }

    public void RepairFinished()
    {
        IsBroke = false;
        IsRepairing = false;
        PowerOn();
    }

    public void Break()
    {
        IsBroke = true;
        PowerOff();
    }
}