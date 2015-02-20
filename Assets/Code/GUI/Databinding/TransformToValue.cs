using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using Assets.Code;

public class TransformToValue : MonoBehaviour
{
	public string PropertyName;
	private object _data;
	private PropertyInfo _property;

	// Use this for initialization
	void Start()
	{
		_data = gameObject.GetDataContext();
		_property = _data.GetProperty(PropertyName);
		if (_property.PropertyType != typeof(float))
		{
			throw new Exception("Property to read is not of type float");
		}
	}

	void Update()
	{
	    var propertyValue = _property.GetValue(_data, null);
		transform. = new Vector3(Mathf.Max(GetOutputInUnitInterval(), 0.02f), 1, 1);
	}

	public float GetOutputInUnitInterval()
	{
		return (_machineType.Output - _machineType.MinOutput) / (_machineType.OverloadOutput - _machineType.MinOutput);
	}
}
