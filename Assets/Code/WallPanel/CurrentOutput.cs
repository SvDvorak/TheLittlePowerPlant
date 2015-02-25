using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentOutput : MonoBehaviour
{
    public ScoreUpdater ScoreUpdater;
    public Text CityValueText;
    public Text MinOutputText;
    public Text IncomeText;
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
        CityValueText.text = "City value: " + (int)ScoreUpdater.CityValue + "$";
        MinOutputText.text = "Output: " + (int)ScoreUpdater.MinimumOutputRequired + "kw";
	    IncomeText.text = "Income: " + (int) ScoreUpdater.Income + "$";
        MinOutput.localScale = new Vector3(ScoreUpdater.MinimumOutputRequired / ScoreUpdater.MaxOutput * MaxScale, MinOutput.localScale.y, MinOutput.localScale.z);
        Output.localScale = new Vector3(ScoreUpdater.Output / ScoreUpdater.MaxOutput * MaxScale, Output.localScale.y, Output.localScale.z);
	}
}
