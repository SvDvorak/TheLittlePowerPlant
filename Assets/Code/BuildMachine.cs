using UnityEngine;
using System.Collections;

public class BuildMachine : MonoBehaviour
{
    public GameObject MachineSelectionUI;
    private GameObject _machineSelection;

    // Use this for initialization
	void Start ()
	{

	}

    // Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if(_machineSelection == null)
        {
            _machineSelection = (GameObject)Instantiate(MachineSelectionUI);
            _machineSelection.transform.SetParent(transform);
            _machineSelection.transform.localPosition = new Vector3(0, 5, 0);
        }
        else
        {
            Destroy(_machineSelection);
        }
    }
}
