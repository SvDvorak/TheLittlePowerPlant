using System;
using UnityEngine;
using System.Collections;
using Assets.Code;
using Object = UnityEngine.Object;

public class Destroyable : MonoBehaviour, IDamageable
{
    public float InitialHealth { get; private set; }

    private Type _crashAnimationType;
    private float _currentHealth;
    private float _itemValue;

    public void Init(float itemValue, float health, Type type)
    {
        _itemValue = itemValue;
        _crashAnimationType = type;
        InitialHealth = health;
        _currentHealth = InitialHealth;
    }

    public void DoDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth < 0)
        {
            AddCrashAnimation();
            ScoreManager.IncreaseDestructionValue(_itemValue);
        }
    }

    private void AddCrashAnimation()
    {
        if (gameObject.GetComponent(_crashAnimationType) == null)
        {
            gameObject.AddComponent(_crashAnimationType);
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(this);
        }
    }
}