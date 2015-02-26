using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ForcedOverload : MonoBehaviour
{
	private bool _overloaded;

	public void ForceOverload()
	{
		if (animation.IsPlaying("Flip"))
		{
			return;
		}

		HandleAnimation();
		OverloadAllMachines();
	}

	private void HandleAnimation()
	{
		var speed = 1;
		var time = 0f;

		if (_overloaded)
		{
			speed = -1;
			time = animation["Flip"].length;
		}

		animation["Flip"].speed = speed;
		animation["Flip"].time = time;
		animation.Play("Flip");
		_overloaded = !_overloaded;
	}

	private void OverloadAllMachines()
	{
		var machineProcesses = new List<IMachineProcess>()
			.Concat(FindObjectsOfType<CoalProcess>())
			.Concat(FindObjectsOfType<TurbineProcess>())
			.Concat(FindObjectsOfType<NuclearProcess>());

		foreach (var machineProcess in machineProcesses)
		{
			machineProcess.Overload();
		}
	}
}