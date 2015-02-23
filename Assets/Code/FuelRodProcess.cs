using UnityEngine;
using Assets.Code;
using UnityEngine.UI;

public class FuelRodProcess : MonoBehaviour
{
	public Color FreshRodColor;
	public Color FullyDegradedRodColor;
	public int FuelRodIndex = 0;

	private Image _image;
	private Nuclear _nuclearMachine;

	void Start ()
	{
		_nuclearMachine = gameObject.GetDataContext<Nuclear>();
		_image = GetComponent<Image>();
	}

	void Update ()
	{
		var fuelRod = _nuclearMachine.FuelRods[FuelRodIndex];
		_image.color = Color.Lerp(FreshRodColor, FullyDegradedRodColor, fuelRod.Degradation);
	}

	public void ButtonSwapRod()
	{
		SendMessageUpwards("SwapRod", FuelRodIndex, SendMessageOptions.RequireReceiver);
	}
}
