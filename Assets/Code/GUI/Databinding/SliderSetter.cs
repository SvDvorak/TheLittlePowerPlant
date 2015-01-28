using UnityEngine;
using System.Linq;
using System.Reflection;
using Assets.Code;
using UnityEngine.UI;

public class SliderSetter : MonoBehaviour
{
    public string PropertyName;
    private object _data;
    private PropertyInfo _property;

    void Start()
    {
        _data = gameObject.GetDataContext();
        _property = _data.GetProperty(PropertyName);
    }

    void Update()
    {
        var sliderValue = GetComponent<Slider>().value;
        _property.SetValue(_data, sliderValue, null);
    }
}
