using UnityEngine;
using System.Collections;

public class HouseCrash : MonoBehaviour
{
	private const float CrashSpeed = 4;

	void Start ()
	{
	    var smoke = (GameObject)Instantiate(Resources.Load("CrumbleSmoke"), transform.position, transform.rotation);
	    var bounds = gameObject.GetComponent<Renderer>().bounds.size;
	    smoke.transform.localScale = bounds;
	    LeanTween
            .moveY(gameObject, -bounds.y, CrashSpeed)
            .setEase(LeanTweenType.easeInQuad)
            .setOnComplete(() => StopAndDestroy(smoke));
	}

    private static void StopAndDestroy(GameObject smoke)
    {
        smoke.GetComponent<ParticleSystem>().Stop();
        Destroy(smoke, 4f);
    }
}