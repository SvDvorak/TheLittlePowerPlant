using UnityEngine;
using Assets.Code;

public class Attack : MonoBehaviour
{
    public LaserLogic Weapon;
    public float RechargePerSecond;
    public float ChargeUsePerSecond;
    private GameState _gameState;
    private bool _forceRecharging;

    void Start ()
	{
		_gameState = gameObject.GetDataContext<GameState>();
    }

    void Update ()
	{
        if (!_gameState.IsAlive)
        {
            Weapon.StopFiring();
            return;
        }

        if (_gameState.LaserCharge < 0)
        {
            _forceRecharging = true;
            Weapon.StopFiring();
        }

        if (_forceRecharging)
        {
            if (_gameState.LaserCharge > 0.1f)
            {
                _forceRecharging = false;
            }

            RechargeLaser();
        }
        else if(Input.GetMouseButton(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Weapon.FireAt(hit.point);
            }

            _gameState.LaserCharge -= ChargeUsePerSecond*Time.deltaTime;
        }
        else
        {
            RechargeLaser();
            Weapon.StopFiring();
        }
	}

    private void RechargeLaser()
    {
        _gameState.LaserCharge += RechargePerSecond*Time.deltaTime;
    }
}