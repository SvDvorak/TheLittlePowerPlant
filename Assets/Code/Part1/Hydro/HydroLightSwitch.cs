using UnityEngine;
using System.Collections;
using System.Reflection;
using Assets.Code;

public class HydroLightSwitch : MonoBehaviour
{
	public Material NormalLight;
	public Material DurabilityLowLight;
	public MeshRenderer LightMesh;

	private Hydro _hydro;

	void Start ()
	{
		_hydro = gameObject.GetDataContext<Hydro>();
	}

	void Update ()
	{
		var expectedMaterial = NormalLight;
		if (_hydro.Durability < _hydro.BreakdownRiskArea || _hydro.IsBroke)
		{
			expectedMaterial = DurabilityLowLight;
		}

		if (LightMesh.material != expectedMaterial)
		{
			LightMesh.material = expectedMaterial;
		}
	}
}
