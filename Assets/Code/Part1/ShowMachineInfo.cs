using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Code;
using UnityEngine.EventSystems;

public class ShowMachineInfo : MonoBehaviour
{
	public GameObject InfoCanvas;
	private static GameObject _currentlySelectedCanvas;
	
	public void ShowInfo ()
	{
		if (_currentlySelectedCanvas != InfoCanvas)
		{
			if(_currentlySelectedCanvas != null)
			{
				_currentlySelectedCanvas.SetActive(false);
			}

			_currentlySelectedCanvas = InfoCanvas;
		}

		InfoCanvas.SetActive(!InfoCanvas.activeSelf);
	}
}
