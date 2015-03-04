using System.Collections.Generic;
using System.ComponentModel;

public interface IMachineType
{
	string Name { get; set; }
	int Cost { get; set; }
	float Output { get; set; }
	bool IsPoweredOn { get; }
	bool IsOverloaded { get; set; }
	float MaxOutputPerSecond { get; }
}

public class Coal : IMachineType
{
	public string Name { get; set; }
	public int Cost { get; set; }
	public float Output { get; set; }
	public bool IsPoweredOn { get; private set; }
	public Range IncreasingTempRange { get; set; }
	public Range OptimalTempRange { get; set; }
	public float Temperature { get; set; }
	public bool IsOverloaded { get; set; }
	public float MaxOutputPerSecond { get; private set; }

	public Coal()
	{
		IncreasingTempRange = new Range(0, 0.7f);
		OptimalTempRange = new Range(IncreasingTempRange.High, 1f);
		IsPoweredOn = true;
		MaxOutputPerSecond = 100;
	}

	public void TogglePower()
	{
		if(!IsOverloaded)
		{
			IsPoweredOn = !IsPoweredOn;
		}
	}
}

public class Nuclear : IMachineType, INotifyPropertyChanged
{
	private bool _isOverloaded;
	private float _controlRodDepth;
	public string Name { get; set; }
	public int Cost { get; set; }
	public bool IsPoweredOn { get; private set; }
	public bool IsOverloaded
	{
		get { return _isOverloaded; }
		set
		{
			_isOverloaded = value;
			OnPropertyChanged("IsOverloaded");
			OnPropertyChanged("CanAdjustRequestedOutput");
		}
	}

	public float MaxOutputPerSecond { get; private set; }
	public bool CanAdjustRequestedOutput { get { return !IsOverloaded; } }
	public float Output { get; set; }
	public float MinOutput { get; private set; }
	public float OverloadOutput { get; private set; }

	public float ControlRodDepth
	{
		get { return _controlRodDepth; }
		set
		{
			_controlRodDepth = value;
			OnPropertyChanged("ControlRodDepth");
		}
	}

	public List<FuelRod> FuelRods { get; set; }
	public float Temperature { get; set; }

	public Range NoReactionUnit { get; set; }
	public Range OverHeatUnit { get; set; }
	public float MaxTemperature { get; set; }

	public Nuclear()
	{
		Name = "Nuclear";
		FuelRods = new List<FuelRod>
			{
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod(),
				new FuelRod()
			};
		NoReactionUnit = new Range(0f, 0.3f);
		OverHeatUnit = new Range(0.7f, 1f);
		MaxTemperature = FuelRod.BaseTemperature*FuelRods.Count;
		MaxOutputPerSecond = FuelRod.MaxRodOutput*FuelRods.Count;
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		if(PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

public class FuelRod
{
	public float Degradation { get; set; }
	public float Output { get; set; }
	public float Temperature { get; set; }
	public const float BaseTemperature = 1;
	public const float MaxRodOutput = 10;
}

public class Turbine : IMachineType, INotifyPropertyChanged
{
	private float _requestedOutput;
	private bool _isOverloaded;
	public string Name { get; set; }
	public int Cost { get; set; }
	public float Output { get; set; }

	public float RequestedOutput
	{
		get { return _requestedOutput; }
		set
		{
			_requestedOutput = value;
			OnPropertyChanged("RequestedOutput");
		}
	}

	public bool IsPoweredOn { get; private set; }

	public bool IsOverloaded
	{
		get { return _isOverloaded; }
		set
		{
			_isOverloaded = value;
			OnPropertyChanged("IsOverloaded");
			OnPropertyChanged("CanAdjustRequestedOutput");
		}
	}

	public float MaxOutputPerSecond { get { return OverloadOutput; } }

	public bool CanAdjustRequestedOutput { get { return !IsOverloaded; } }
	public bool IsRepairing { get; private set; }
	public bool IsBroke { get; set; }
	public float Durability { get; set; }

	public float MinOutput { get { return 50; } }
	public float MaxNormalOutput { get { return 100; } }
	public float OverloadOutput { get { return 120; } }

	public Turbine()
	{
		Output = MinOutput;
		RequestedOutput = MinOutput;
		Durability = 1;
		PowerOn();
	}

	public void TogglePower()
	{
		if (IsOverloaded)
		{
			return;
		}

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
		if (!IsBroke)
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
		if(!IsOverloaded)
		{
			IsRepairing = true;
			PowerOff();
		}
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		if(PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}