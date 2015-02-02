using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{
    public Text OutputVelocity;
    public ScoreManager ScoreManager;

    private float _previousIncome = 0;

	void Update ()
	{
	    OutputVelocity.text = "OutputVelocity: " + (ScoreManager.Income - _previousIncome);

	    _previousIncome = ScoreManager.Income;
    }
}
