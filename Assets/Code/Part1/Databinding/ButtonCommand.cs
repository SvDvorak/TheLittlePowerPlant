using UnityEngine;
using Assets.Code;
using System.Collections;
using System.Reflection;
using UnityEngine.UI;

public class ButtonCommand : MonoBehaviour
{
    public string MethodName;

    private object _data;
    private MethodInfo _method;

    private void Start()
    {
        _data = gameObject.GetDataContext();
        _method = _data.GetMethod(MethodName);
        GetComponent<Toggle>().onValueChanged.AddListener(ButtonClicked);
    }

    private void ButtonClicked(bool value)
    {
        _method.Invoke(_data, null);
    }
}