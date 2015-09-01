using UnityEngine;
using System.Collections;

public class ExplosionTouch : MonoBehaviour
{
    private Object _explosion;
    private int _explosionTouchCount;

    void Start ()
	{
        _explosion = Resources.Load("Explosion");
        AddExplosion();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_explosionTouchCount > 10)
        {
            Destroy(this);
        }

        if(collision.gameObject.tag != "Effect")
        {
            AddExplosion();
            _explosionTouchCount++;
        }
    }

    private void AddExplosion()
    {
        Instantiate(_explosion, transform.position, transform.rotation);
    }
}