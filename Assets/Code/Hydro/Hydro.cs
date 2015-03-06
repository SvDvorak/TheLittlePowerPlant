using System.ComponentModel;

public class Hydro : IMachineType, INotifyPropertyChanged
{
	private float _requestedFlow;
	private bool _isOverloaded;
	public string Name { get; set; }
	public int Cost { get; set; }
	public float Output { get; set; }
	public float CurrentFlow { get; set; }

	public float RequestedFlow
	{
		get { return _requestedFlow; }
		set
		{
			_requestedFlow = value;
			OnPropertyChanged("RequestedFlow");
		}
	}
	public Range CurrentFlowRange { get { return new Range(0, CurrentFlowUnit); } }
	public float CurrentFlowUnit { get { return (CurrentFlow - MinFlow)/(MaxFlow - MinFlow); } }
	public float MaxFlow { get { return 120; } }
	public float MinFlow { get { return 50; } }
	public float MaxNormalFlow { get { return 100; } }
	public float OverloadFlow { get { return 120; } }
	public float MaxOutputPerSecond { get { return 45; } }


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

	public bool CanAdjustRequestedOutput { get { return !IsOverloaded; } }
	public bool IsRepairing { get; private set; }
	public bool IsBroke { get; set; }
	public float Durability { get; set; }
	public float BreakdownRiskArea { get { return 0.5f; } }

	public Hydro()
	{
		RequestedFlow = MaxNormalFlow;
		CurrentFlow = RequestedFlow;
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
			CurrentFlow = MinFlow;
		}
	}

	private void PowerOff()
	{
		IsPoweredOn = false;
		CurrentFlow = 0;
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