using UnityEngine;
using System.Collections;
using Assets.Code;

public class UpdateFireGlow : MonoBehaviour
{
	private Light _light;
	private Coal _coal;
	private const int MaxIntensity = 8;

	void Start ()
	{
		_coal = gameObject.GetDataContext<Coal>();
		_light = GetComponent<Light>();
	}

	void Update ()
	{
		_light.intensity = _coal.Temperature*MaxIntensity;
	}
}