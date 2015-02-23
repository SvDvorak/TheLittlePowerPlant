using UnityEngine;
using System.Collections;
using Assets.Code;
using UnityEngine.UI;

public class FuelRodProcess : MonoBehaviour
{
	public Color FreshRodColor;
	public Color FullyDegradedRodColor;
	public int FuelRodIndex = 0;

	private const float MaxRodOutput = 10;
	private FuelRod _fuelRod;
	private Image _image;
	private Nuclear _nuclearMachine;

	private const float DegradationPerSecond = 0.01f;
	private const float MaxTemperatureShift = 0.2f;

	void Start ()
	{
		_nuclearMachine = gameObject.GetDataContext<Nuclear>();
		_fuelRod = _nuclearMachine.FuelRods[FuelRodIndex];
		_image = GetComponent<Image>();
	}

	void Update ()
	{
		_fuelRod.Degradation += DegradationPerSecond * Time.deltaTime;
		_image.color = Color.Lerp(FreshRodColor, FullyDegradedRodColor, _fuelRod.Degradation);

		var degradationInverse = 1 - _fuelRod.Degradation;
		_fuelRod.Output = degradationInverse*MaxRodOutput;

		var rodTemperatureShift = MaxTemperatureShift*degradationInverse;
        _fuelRod.Temperature = FuelRod.BaseTemperature*degradationInverse + Random.Range(-rodTemperatureShift, rodTemperatureShift);
	}

	public void SwapRod()
	{
		_fuelRod = new FuelRod();
		_nuclearMachine.FuelRods[FuelRodIndex] = _fuelRod;
	}
}
