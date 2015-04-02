using UnityEngine;
using System.Collections;

public class HelicopterSpawner : MonoBehaviour
{
	public GameObject HelicopterTemplate;
	public GameObject AttackTarget;
	public float MaxOffsetInX;
	public float HeightOffset;
	public float SpawnTime;
	private float _timeSinceLastSpawn;
	private float _timeTillNextSpawn;

	void Start ()
	{
		_timeTillNextSpawn = GetNextSpawnTime();
	}
	
	void Update ()
	{
		if (_timeSinceLastSpawn > _timeTillNextSpawn)
		{
			_timeSinceLastSpawn = 0;
			_timeTillNextSpawn = GetNextSpawnTime();
			SpawnHelicopter();
		}

		_timeSinceLastSpawn += Time.deltaTime;
	}

	private float GetNextSpawnTime()
	{
		return SpawnTime + UnityEngine.Random.Range(-2, 2);
	}

	[ContextMenu("Spawn")]
	private void SpawnHelicopter()
	{
		var helicopter = Instantiate(HelicopterTemplate);
		helicopter.transform.position = AttackTarget.transform.position + new Vector3(UnityEngine.Random.Range(-MaxOffsetInX, MaxOffsetInX), HeightOffset, -120);
		helicopter.GetComponent<HelicopterAI>().AttackTarget = AttackTarget;
	}
}