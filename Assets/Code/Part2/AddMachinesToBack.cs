using UnityEngine;
using System.Collections;

public class AddMachinesToBack : MonoBehaviour
{
	public GameObject HydroTemplate;
	public GameObject NuclearTemplate;
	public GameObject CoalTemplate;

	public GameObject MachineBackCenter;

	private const float MachineSeparation = 2.5f;
	private const float SideTiltHeight = 0.3f;

	void Start ()
	{
		ScoreManager.MachineBuilt(new Hydro(), new Vector3(0, 0));
		ScoreManager.MachineBuilt(new Coal(), new Vector3(1, 0, 1));
		ScoreManager.MachineBuilt(new Nuclear(), new Vector3(1, 0));

		for (int i = 0; i < ScoreManager.BuiltMachines.Count; i++)
		{
			var machineType = ScoreManager.BuiltMachines[i].MachineType;
			var machinePosition = ScoreManager.BuiltMachines[i].Position;
			var machine = new GameObject();
			if (machineType is Hydro)
			{
				machine = Instantiate(HydroTemplate);
			}
			else if (machineType is Nuclear)
			{
				machine = Instantiate(NuclearTemplate);
			}
			else if (machineType is Coal)
			{
				machine = Instantiate(CoalTemplate);
			}

			machine.transform.SetParent(MachineBackCenter.transform, false);
			machine.transform.Translate(ToWeirdBackPlacementSystem(ScoreManager.BuiltMachines[i].Position*MachineSeparation + new Vector3(0, -SideTiltHeight*Mathf.Abs(machinePosition.x))));
			machine.transform.Rotate(new Vector3(90, 0, 0));
			machine.transform.Rotate(new Vector3(0, 90, 0));
			machine.transform.localScale = Vector3.one*0.75f;
		}
	}

	public Vector3 ToWeirdBackPlacementSystem(Vector3 vector)
	{
		return new Vector3(vector.z, vector.x, vector.y);
	}
}