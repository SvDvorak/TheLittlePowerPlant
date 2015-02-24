using System;
using Assets.Code;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildMachine : MonoBehaviour
{
    public GameObject MachineSelectionTemplate;
    public GameObject TurbineTemplate;
    public GameObject NuclearTemplate;
    public GameObject CoalTemplate;
	public ScoreManager OutputManager;
    private GameObject _machineSelection;
	private GameObject _builtMachine;

    public void TileSelected()
    {
        if(_machineSelection == null && _builtMachine == null)
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

            _builtMachine = CreateMachine(machineTypeToBuild);
        }
    }

    private GameObject CreateMachine(IMachineType machineTypeToBuild)
    {
        var machine = new GameObject();
        if(machineTypeToBuild is Turbine)
        {
            machine = (GameObject)Instantiate(TurbineTemplate);
            machine.GetComponent<TurbineProcess>().Initialize(OutputManager, machineTypeToBuild);
        }
        else if(machineTypeToBuild is Nuclear)
        {
            machine = (GameObject)Instantiate(NuclearTemplate);
            machine.GetComponent<NuclearProcess>().Initialize(OutputManager, machineTypeToBuild);
        }
		else if (machineTypeToBuild is Coal)
		{
			machine = (GameObject)Instantiate(CoalTemplate);
			machine.GetComponent<CoalProcess>().Initialize(OutputManager, machineTypeToBuild);
		}

		machine.transform.SetParent(transform, false);
        OutputManager.Income -= machineTypeToBuild.Cost;

	    return machine;
    }
}
