using System;
using Assets.Code;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildMachine : MonoBehaviour
{
    public GameObject MachineSelectionTemplate;
    public GameObject HydroTemplate;
    public GameObject NuclearTemplate;
    public GameObject CoalTemplate;
	public ScoreUpdater OutputUpdater;
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
        if(OutputUpdater.Income > machineTypeToBuild.Cost)
        {
            Destroy(_machineSelection);

            _builtMachine = CreateMachine(machineTypeToBuild);
        }
    }

    private GameObject CreateMachine(IMachineType machineTypeToBuild)
    {
        var machine = new GameObject();
        if(machineTypeToBuild is Hydro)
        {
            machine = (GameObject)Instantiate(HydroTemplate);
            machine.GetComponent<HydroProcess>().Initialize(OutputUpdater, machineTypeToBuild);
        }
        else if(machineTypeToBuild is Nuclear)
        {
            machine = (GameObject)Instantiate(NuclearTemplate);
            machine.GetComponent<NuclearProcess>().Initialize(OutputUpdater, machineTypeToBuild);
        }
		else if (machineTypeToBuild is Coal)
		{
			machine = (GameObject)Instantiate(CoalTemplate);
			machine.GetComponent<CoalProcess>().Initialize(OutputUpdater, machineTypeToBuild);
		}

		machine.transform.SetParent(transform, false);
        OutputUpdater.Income -= machineTypeToBuild.Cost;

	    return machine;
    }
}
