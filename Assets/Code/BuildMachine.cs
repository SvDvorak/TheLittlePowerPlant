using System;
using UnityEngine;

public class BuildMachine : MonoBehaviour
{
    public GameObject MachineSelectionTemplate;
    public GameObject TurbineTemplate;
    public GameObject NuclearTemplate;
    public ScoreManager OutputManager;
    private GameObject _machineSelection;
    private GameObject _plateModel;

	void Start ()
	{
	    _plateModel = transform.FindChild("Model").gameObject;
	}

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

            CreateMachine(machineTypeToBuild);
        }
    }

    private void CreateMachine(IMachineType machineTypeToBuild)
    {
        var machine = new GameObject();
        if(machineTypeToBuild is Turbine)
        {
            machine = (GameObject) Instantiate(TurbineTemplate);
            machine.GetComponent<TurbineProcess>().Initialize(OutputManager, machineTypeToBuild);
        }
        else if(machineTypeToBuild is Nuclear)
        {
            machine = (GameObject)Instantiate(NuclearTemplate);
            machine.GetComponent<NuclearProcess>().Initialize(OutputManager, machineTypeToBuild);
        }

        machine.transform.SetParent(transform, false);
        OutputManager.Income -= machineTypeToBuild.Cost;
    }
}
