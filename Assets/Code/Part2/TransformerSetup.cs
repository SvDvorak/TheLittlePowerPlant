using UnityEngine;
using System.Collections;

public class TransformerSetup : MonoBehaviour
{
	public DataContext GuiDataContext;

	void Awake ()
	{
		var transformerState = new GameState();
		GetComponent<DataContext>().Data = transformerState;
		GuiDataContext.Data = transformerState;
	}
}