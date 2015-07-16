using System;
using System.Reflection;
using UnityEngine;
using Assets.Code;

public class RangeToDeltaSize : MonoBehaviour
{
	public string RangePropertyName;
	public bool IsHorizontal;
	private object _data;
	private PropertyInfo _rangeProperty;
	private RectTransform _containerSize;
	private RectTransform _rectTransform;

	void Start ()
	{
		_data = gameObject.GetDataContext();
		_rangeProperty = _data.GetProperty(RangePropertyName);
		_rectTransform = GetComponent<RectTransform>();
		_containerSize = transform.parent.GetComponent<RectTransform>();
		if (_rangeProperty.PropertyType != typeof(Range))
		{
			throw new Exception("Range property is not of type Range.");
		}
	}

	public void OnGUI()
	{
		var range = _rangeProperty.GetValue<Range>(_data);

		if (IsHorizontal)
		{
			var value = (range.High - range.Low) * _containerSize.sizeDelta.x;
			_rectTransform.sizeDelta = new Vector2(value, _rectTransform.sizeDelta.y);
		}
		else
		{
			var value = (range.High - range.Low) * _containerSize.sizeDelta.y;
			_rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, value);
		}
	}
}
