using UnityEngine;
using System.Collections;
using Assets.Code;

public class UpdateAnimatorOnMachineState : MonoBehaviour
{
	public Animator PowerOnAnimator;
	private Hydro _hydro;

	void Start ()
	{
		_hydro = gameObject.GetDataContext<Hydro>();
	}

	void Update ()
	{
		var matchesState = PowerOnAnimator.GetBool("IsPoweredOn") == _hydro.IsPoweredOn;
		if (!matchesState)
		{
			PowerOnAnimator.SetBool("IsPoweredOn", _hydro.IsPoweredOn);
		}

		PowerOnAnimator.SetFloat("Effect", _hydro.CurrentFlowUnit);
	}
}
