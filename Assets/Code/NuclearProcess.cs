using UnityEngine;
using System.Collections;

public class NuclearProcess : MonoBehaviour
{
	void Start ()
	{
		Initialize(new ScoreManager(), new Nuclear());
	}

	public void Initialize(ScoreManager outputManager, IMachineType machineType)
	{
		GetComponentInChildren<DataContext>().Data = machineType;
	}

	void Update ()
	{
	}
}
