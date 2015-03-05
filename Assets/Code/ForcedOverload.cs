using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ForcedOverload : MonoBehaviour
{
	public bool Overloaded;
	private Animation _animation;

	public void Start()
	{
		_animation = GetComponent<Animation>();
	}

	public void ForceOverload()
	{
		if (_animation.IsPlaying("Flip"))
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

		if (Overloaded)
		{
			speed = -1;
			time = _animation["Flip"].length;
		}

		_animation["Flip"].speed = speed;
		_animation["Flip"].time = time;
		_animation.Play("Flip");
		Overloaded = !Overloaded;
	}

	private void OverloadAllMachines()
	{
		var machineProcesses = new List<IMachineProcess>()
			.Concat(FindObjectsOfType<CoalProcess>())
			.Concat(FindObjectsOfType<HydroProcess>())
			.Concat(FindObjectsOfType<NuclearProcess>());

		foreach (var machineProcess in machineProcesses)
		{
			machineProcess.Overload();
		}
	}
}