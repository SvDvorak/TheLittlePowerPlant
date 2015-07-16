using System.Linq;
using System.Reflection;
using Assets.Code;
using UnityEngine;
using UnityEngine.UI;

public class TextReader : MonoBehaviour
{
    public string PropertyName;
    public string Format;
    private object _data;
    private PropertyInfo _property;

    // Use this for initialization
	void Start ()
	{
	    _data = gameObject.GetDataContext();
	    _property = _data.GetProperty(PropertyName);
	}

    // Update is called once per frame
	void Update ()
	{
	    var propertyValue = _property.GetValue(_data, null);

	    string output;
	    if (!string.IsNullOrEmpty(Format))
	    {
	        output = string.Format(Format, propertyValue);
	    }
	    else
	    {
	        output = propertyValue.ToString();
	    }

	    GetComponent<Text>().text = output;
	}
}