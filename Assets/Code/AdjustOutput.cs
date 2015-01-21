using UnityEngine;
using UnityEngine.UI;

public class AdjustOutput : MonoBehaviour
{
    public Slider SelectedOutput;
    public RectTransform ActualOutput;

    public float OutputAdjustPerSecond = 5;

	void Start ()
    {
	
	}
	
	void Update ()
	{
	    var outputChangeMaxDelta = (OutputAdjustPerSecond*Time.deltaTime)/100f;
	    var newOutput = Mathf.MoveTowards(ActualOutput.localScale.x, SelectedOutput.value, outputChangeMaxDelta);
	    ActualOutput.localScale = new Vector3(Mathf.Max(newOutput, 0.02f), 1, 1);
	}
}
