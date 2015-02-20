using System.Collections.Generic;
using UnityEngine;

public interface IMachineType
{
    string Name { get; set; }
    int Cost { get; set; }
    bool IsPoweredOn { get; }
    float Output { get; set; }
    float MinOutput { get; }
    float OverloadOutput { get; }

    void TogglePower();
    void Repair();
}

public class Nuclear : IMachineType
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public bool IsPoweredOn { get; private set; }
    public float Output { get; set; }
    public float MinOutput { get; private set; }
    public float OverloadOutput { get; private set; }
	public float ControlRodDepth { get; set; }

	public List<FuelRod> FuelRods { get; set; }
	public float Temperature { get; set; }

	public Nuclear()
	{
		Name = "Nuclear";
		FuelRods = new List<FuelRod>()
			{
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
			};
		IsPoweredOn = true;
	}

	public void TogglePower()
	{
		IsPoweredOn = !IsPoweredOn;
	}

    public void Repair()
    {
        throw new System.NotImplementedException();
    }

	public void PowerOff()
	{
		IsPoweredOn = false;
	}
}

public class FuelRod
{
	public float Degradation { get; set; }
	public float Output { get; set; }
	public float Temperature { get; set; }
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
    public float MaxNormalOutput { get { return 100; } }
    public float OverloadOutput { get { return 120; } }

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