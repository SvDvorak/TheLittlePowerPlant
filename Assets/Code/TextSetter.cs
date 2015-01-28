using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
    public string PropertyName;
    private object _data;
    private PropertyInfo _property;

    // Use this for initialization
	void Start ()
	{
	    var dataContext = (DataContext) gameObject.GetComponentInParent(typeof (DataContext));
	    _data = dataContext.Data;
	    _property = _data.GetType().GetProperties().Single(x => x.Name == PropertyName);
	}

    // Update is called once per frame
	void Update ()
	{
	    var propertyValue = (string)_property.GetValue(_data, null);
	    GetComponent<Text>().text = propertyValue;
	}
}