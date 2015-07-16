using UnityEngine;
using System.Collections;
using Assets.Code;

public class Attack : MonoBehaviour
{
    public LaserLogic Weapon;
    private GameState _gameState;

    void Start ()
	{
		_gameState = gameObject.GetDataContext<GameState>();
    }

    void Update ()
	{
        if (_gameState.IsAlive && Input.GetMouseButton(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Weapon.FireAt(hit.point);
            }
        }
    }
}