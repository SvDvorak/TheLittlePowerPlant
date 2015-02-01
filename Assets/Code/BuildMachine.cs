using System;
using UnityEngine;

public class BuildMachine : MonoBehaviour
{
    public GameObject MachineSelectionTemplate;
    public GameObject TurbineTemplate;
    public ScoreManager OutputManager;
    private GameObject _machineSelection;
    private GameObject _plateModel;

    // Use this for initialization
	void Start ()
	{
	    _plateModel = transform.FindChild("Model").gameObject;
	}

    // Update is called once per frame
	void Update ()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
           if(hit.collider.gameObject == _plateModel)
           {
               TileSelected();
           }
        }
	}

    void TileSelected()
    {
        if(_machineSelection == null)
        {
            _machineSelection = (GameObject)Instantiate(MachineSelectionTemplate);
            _machineSelection.transform.SetParent(transform);
            _machineSelection.transform.localPosition = new Vector3(0, 2.5f, 0);
            var machineSelectionList = _machineSelection.GetComponent<MachineSelectionList>();
            machineSelectionList.MachineSelected += Build;
        }
        else
        {
            Destroy(_machineSelection);
        }
    }

    private void Build(IMachineType machineTypeToBuild)
    {
        if(OutputManager.Income > machineTypeToBuild.Cost)
        {
            Destroy(_machineSelection);

            var machine = (GameObject)Instantiate(TurbineTemplate);
            machine.transform.SetParent(transform, false);
            machine.GetComponent<MachineSetup>().Initialize(OutputManager, machineTypeToBuild);
            OutputManager.Income -= machineTypeToBuild.Cost;
        }
    }
}
