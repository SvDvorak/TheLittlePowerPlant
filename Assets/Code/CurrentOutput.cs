using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentOutput : MonoBehaviour
{
    public Text CityValueText;
    public Text MinOutputText;
    public Transform MinOutput;
    public Transform Output;
    public float MaxOutput = 400;
    public float MaxScale = 3.9f;
    private float _currentCityValue;
    private float _currentOutput = 200;

    void Start ()
    {
        //_line = new GameObject("Output").AddComponent<LineRenderer>();
        //_line.SetVertexCount(2);
        //_line.SetWidth(.025f, .025f);
        //_line.SetColors(Color.black, Color.black);
        //_line.renderer.material.color = Color.black;
        //_line.gameObject.SetActive(true);
        //_line.SetPosition(1, transform.position + new Vector3(MaxOutput, 0, 0));
        InvokeRepeating("UpdateOutput", 0, 1);
    }

    private void UpdateOutput()
    {
        _currentCityValue += 969 + Random.Range(-3, 3);
        _currentOutput += Random.Range(-5, 6);
        CityValueText.text = "City value: " + (int) _currentCityValue + "$";
        MinOutputText.text = "Output: " + (int)_currentCityValue / 100 + "kw";
        MinOutput.localScale = new Vector3((_currentCityValue / 100) / MaxOutput * MaxScale, MinOutput.localScale.y, MinOutput.localScale.z);
        Output.localScale = new Vector3(_currentOutput / MaxOutput * MaxScale, Output.localScale.y, Output.localScale.z);
        //_line.SetPosition(0, transform.position + new Vector3((_currentPropertyValue / 100) / MaxOutput, 0, 1));
    }

    void FixedUpdate()
    {
    }

	void Update ()
    {
	}
}
