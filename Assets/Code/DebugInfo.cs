using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{
    public Text CityValue;

	void Update ()
	{
	    CityValue.text = "CityValue: " + ScoreManager.CityValue;
    }
}
