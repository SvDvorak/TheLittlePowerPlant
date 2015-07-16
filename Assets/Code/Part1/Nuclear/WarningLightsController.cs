using UnityEngine;
using Assets.Code;

public class WarningLightsController : MonoBehaviour
{
	public Material NormalLight;
	public Material WarningLight;

	private Nuclear _nuclear;
	private MeshRenderer _lightMesh;

	private float _timeSinceLastSwitch;
	private const float SwitchBufferTimeInSeconds = 0.5f;

	void Start()
	{
		_nuclear = gameObject.GetDataContext<Nuclear>();
		_lightMesh = GetComponent<MeshRenderer>();
		_timeSinceLastSwitch = SwitchBufferTimeInSeconds;
	}

	void Update()
	{
		var expectedMaterial = NormalLight;
		if (_nuclear.IsPoweredOn && _nuclear.IsOutsideSafeLimit)
		{
			expectedMaterial = WarningLight;
		}

		_timeSinceLastSwitch += Time.deltaTime;

		if (_timeSinceLastSwitch > SwitchBufferTimeInSeconds)
		{
			_lightMesh.material = expectedMaterial;
			_timeSinceLastSwitch = 0;
		}
	}
}