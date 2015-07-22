using System;
using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour, IDamageable
{
    public float InitialHealth { get; private set; }

    private Type _crashAnimationType;
    private float _currentHealth;

    public void Init(float health, Type type)
    {
        _crashAnimationType = type;
        InitialHealth = health;
        _currentHealth = InitialHealth;
    }

    public void DoDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth < 0)
        {
            AddBuildingCrumbleAnimation();
        }
    }

    private void AddBuildingCrumbleAnimation()
    {
        if (gameObject.GetComponent(_crashAnimationType) == null)
        {
            gameObject.AddComponent(_crashAnimationType);
            Destroy(this);
        }
    }
}