using UnityEngine;
using System.Collections;

public class MomentaryExplosion : MonoBehaviour
{
	void Start ()
	{
        LeanTween
            .scale(gameObject, transform.localScale * 1.7f, 0.12f).setOnComplete(() => LeanTween
            .scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() =>
            EndExplosion()));
    }

    private void EndExplosion()
    {
        LeanTween.cancel(gameObject);
        Destroy(gameObject);
    }
}