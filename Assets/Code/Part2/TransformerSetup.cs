using UnityEngine;
using System.Collections;

public class TransformerSetup : MonoBehaviour
{
	public DataContext GuiDataContext;

	void Start ()
	{
		var transformerState = new TransformerState();
		GetComponent<DataContext>().Data = transformerState;
		GuiDataContext.Data = transformerState;
	}
}