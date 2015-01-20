using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleColorFill : MonoBehaviour
{
    private Toggle _toggle;
    private Image _image;

    // Use this for initialization
	void Start ()
	{
        _toggle = GetComponent<Toggle>();
        _image = GetComponent<Image>();
	}

    // Update is called once per frame
	void Update ()
    {
	
	}

    void OnGUI()
    {
        if (_toggle.isOn)
        {
            _image.color = _toggle.colors.pressedColor;
        }
        else
        {
            _image.color = _toggle.colors.normalColor;
        }
    }
}