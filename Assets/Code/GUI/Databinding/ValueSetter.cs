using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Assets.Code;
using UnityEngine.UI;

public class ValueSetter : MonoBehaviour
{
    public string ComponentTypeName;
    public string ComponentPropertyName;
    public string DataContextPropertyName;

    private Func<object> _readComponentProperty;
    private Action<object> _setDataContextProperty;

    void Start()
    {
        var component = GetComponent(ComponentTypeName);
        var componentProperty = component.GetProperty(ComponentPropertyName);
        _readComponentProperty = () => componentProperty.GetValue(component, null);

        var data = gameObject.GetDataContext();
        var dataContextProperty = data.GetProperty(DataContextPropertyName);
        _setDataContextProperty = x => dataContextProperty.SetValue(data, x, null);
    }

    void Update()
    {
        _setDataContextProperty(_readComponentProperty());
    }
}
