using System.ComponentModel;

public class Hydro : IMachineType, INotifyPropertyChanged
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

	public Hydro()
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