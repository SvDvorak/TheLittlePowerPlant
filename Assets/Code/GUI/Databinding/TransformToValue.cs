using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using Assets.Code;

public class TransformToValue : MonoBehaviour
{
	public string ValuePropertyName;
	public string MaxValuePropertyName;
	public float MaxHeight;
	private object _data;
	private PropertyInfo _valueProperty;
	private PropertyInfo _maxValueProperty;
	private RectTransform _transform;

	// Use this for initialization
	void Start()
	{
		_data = gameObject.GetDataContext();
		_valueProperty = _data.GetProperty(ValuePropertyName);
		_maxValueProperty = _data.GetProperty(MaxValuePropertyName);
		if (_valueProperty.PropertyType != typeof(float) || _maxValueProperty.PropertyType != typeof(float))
		{
			throw new Exception("Properties are not of type float.");
		}
		_transform = GetComponent<RectTransform>();
	}

	void Update()
	{
	    var value = (float)_valueProperty.GetValue(_data, null);
	    var maxValue = (float)_maxValueProperty.GetValue(_data, null);
		_transform.sizeDelta = new Vector2(1, (value/maxValue)*MaxHeight);
	}
}
