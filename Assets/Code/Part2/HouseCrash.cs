using UnityEngine;
using System.Collections;

public class HouseCrash : MonoBehaviour
{
	private const float CrashSpeed = 4;

	void Start ()
	{
		LeanTween.moveY(gameObject, -16, CrashSpeed).setEase(LeanTweenType.easeInQuad);
	}
}