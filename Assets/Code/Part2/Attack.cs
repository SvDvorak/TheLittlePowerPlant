using UnityEngine;
using Assets.Code;

public class Attack : MonoBehaviour
{
    public LaserLogic Weapon;
    public Renderer WeaponRenderer;
    public float RechargePerSecond;
    public float ChargeUsePerSecond;
    private GameState _gameState;
    private bool _forceRecharging;
    private Color _emissionColor;
    private readonly Color _noChargeLaserColor = new Color(0.1f, 0.1f, 0.1f);

    void Start ()
    {
        _gameState = gameObject.GetDataContext<GameState>();
        _emissionColor = WeaponRenderer.material.GetColor("_EmissionColor");
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

        var newEmissionColor = Color.Lerp(_noChargeLaserColor, _emissionColor, _gameState.LaserCharge);
        WeaponRenderer.material.SetColor("_EmissionColor", newEmissionColor);
        DynamicGI.SetEmissive(WeaponRenderer, newEmissionColor);

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