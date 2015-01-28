using UnityEngine;

public interface IMachineType
{
    string Name { get; set; }
    float Output { get; set; }
    float MinOutput { get; }
    float MaxOutput { get; }
}

public class Turbine : IMachineType
{
    public string Name { get; set; }
    public float Output { get; set; }
    public float RequestedOutput { get; set; }

    public float MinOutput { get { return 50; } }
    public float MaxOutput { get { return 150; } }

    public Turbine()
    {
        Output = MinOutput;
    }
}