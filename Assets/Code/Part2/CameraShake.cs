using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	private LTDescr _easeOutTween;
	private LTDescr _shakeTween;
	private float _currentShaking;
	public float ShakeAmt = 0.2f; // the degrees to shake the camera
	public float ShakePeriodTime = 0.12f; // The period of each shake
	public float DropOffTime = 1.6f; // How long it takes the shaking to settle down to nothing

	private void Start()
	{
		_shakeTween = LeanTween.rotateAroundLocal(gameObject, Vector3.right, 0, ShakePeriodTime)
			.setEase(LeanTweenType.easeShake)
			.setLoopClamp()
			.setRepeat(-1);
	}

	public void AddCollisionForce(float force)
	{
		var newShaking = ShakeAmt*force;
		if (newShaking < _currentShaking)
		{
			return;
		}

		if (_easeOutTween != null)
		{
			_easeOutTween.cancel();
		}

		_easeOutTween = LeanTween.value(gameObject, ShakeAmt*force, 0f, DropOffTime)
			.setOnUpdate(val =>
				{
					_currentShaking = val;
					_shakeTween.setTo(Vector3.right * val);
				})
			.setEase(LeanTweenType.easeOutQuad);
	}
}