using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Code;
using UnityEngine.EventSystems;

public class ShowMachineInfo : MonoBehaviour
{
	public GameObject InfoCanvas;
	
	public void ShowInfo ()
	{
		InfoCanvas.SetActive(!InfoCanvas.activeSelf);
	}
}
