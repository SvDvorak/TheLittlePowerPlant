using System;
using System.ComponentModel;
using Assets.Code;
using UnityEngine;

public class ValueBinding : MonoBehaviour
{
    public string ComponentTypeName;
    public string ComponentPropertyName;
    public string DataContextPropertyName;

    private Func<object> _readComponentProperty;
	private Action<object> _setComponentProperty;
    private Action<object> _setDataContextProperty;
    private Func<object> _readDataContextProperty;
	private INotifyPropertyChanged _notifyingData;
	private object _dataContextValue;
	private object _componentValue;

	void Start()
    {
        var component = GetComponent(ComponentTypeName);
        var componentProperty = component.GetProperty(ComponentPropertyName);
        _readComponentProperty = () => componentProperty.GetValue(component, null);
		_setComponentProperty = x => componentProperty.SetValue(component, x, null);

		var data = gameObject.GetDataContext();
		_notifyingData = data as INotifyPropertyChanged;
		if (_notifyingData == null)
	    {
		    throw new Exception("Function requires that DataContext " + data.GetType().Name + " inherits from INotifyPropertyChanged.");
	    }

		_notifyingData.PropertyChanged += PropertyChanged;
	    var dataContextProperty = _notifyingData.GetProperty(DataContextPropertyName);
        _readDataContextProperty = () => dataContextProperty.GetValue(_notifyingData, null);

		if(dataContextProperty.GetSetMethod() != null)
		{
			_setDataContextProperty = x => dataContextProperty.SetValue(_notifyingData, x, null);
		}
    }

	void Update()
	{
		_componentValue = _readComponentProperty();
		if (!_componentValue.Equals(_dataContextValue) && _setDataContextProperty != null)
		{
			_setDataContextProperty(_componentValue);
		}
	}

	void OnDestroy()
	{
		if(_notifyingData != null)
		{
			_notifyingData.PropertyChanged -= PropertyChanged;
		}
	}

	private void PropertyChanged(object sender, PropertyChangedEventArgs propertyArgs)
	{
		if (propertyArgs.PropertyName == DataContextPropertyName)
		{
			_dataContextValue = _readDataContextProperty();
			if (!_dataContextValue.Equals(_componentValue))
			{
				_setComponentProperty(_dataContextValue);
			}
		}
	}
}
