using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentOutput : MonoBehaviour
{
    public ScoreManager ScoreManager;
    public Text CityValueText;
    public Text MinOutputText;
    public Transform MinOutput;
    public Transform Output;
    public float MaxScale = 3.9f;

    void Start ()
    {
    }

    void FixedUpdate()
    {
    }

	void Update ()
    {
        CityValueText.text = "City value: " + (int)ScoreManager.CityValue + "$";
        MinOutputText.text = "Output: " + (int)ScoreManager.MinimumOutputRequired + "kw";
        MinOutput.localScale = new Vector3(ScoreManager.MinimumOutputRequired / ScoreManager.MaxOutput * MaxScale, MinOutput.localScale.y, MinOutput.localScale.z);
        Output.localScale = new Vector3(ScoreManager.Output / ScoreManager.MaxOutput * MaxScale, Output.localScale.y, Output.localScale.z);
	}
}
