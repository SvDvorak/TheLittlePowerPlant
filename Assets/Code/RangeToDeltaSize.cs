using System;
using System.Reflection;
using UnityEngine;
using Assets.Code;

public class RangeToDeltaSize : MonoBehaviour
{
	public string RangePropertyName;
	public float MaxSize;
	public bool IsHorizontal;
	private object _data;
	private PropertyInfo _rangeProperty;

	void Start ()
	{
		_data = gameObject.GetDataContext();
		_rangeProperty = _data.GetProperty(RangePropertyName);
		if (_rangeProperty.PropertyType != typeof(Range))
		{
			throw new Exception("Range property is not of type Range.");
		}
	}

	public void OnGUI()
	{
		var range = _rangeProperty.GetValue<Range>(_data);

		var transform = GetComponent<RectTransform>();
		var value = (range.High - range.Low) * MaxSize;
		transform.sizeDelta = IsHorizontal ? new Vector2(value, 0) : new Vector2(0, value);
	}
}
