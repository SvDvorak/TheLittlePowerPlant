using UnityEngine;

public class HelicopterAI : MonoBehaviour, IDamageable
{
	public GameObject AttackTarget;
	public float FlySpeed;
	public float MaxSpeedDistance;
	public float TargetMinDistance;
	public float TargetMaxDistance;
	public float NewOffsetTimeInSeconds;
	public float OffsetConeFromTarget;
	public float FireDelay;
	public GameObject MissileTemplate;
	public Transform MissileSpawnL;
	public Transform MissileSpawnR;

	private Vector3 _offset;
	private bool _fireFromLeft;
	private Vector3 _previousTargetPosition;
	private Vector3 _targetMovement;
    private float _currentHealth;

    [SerializeField]
    private float _initialHealth;
    public float InitialHealth { get { return _initialHealth; } }

    void Start ()
	{
        _currentHealth = InitialHealth;
        InvokeRepeating("SetNewRandomOffset", 0, NewOffsetTimeInSeconds);
		InvokeRepeating("FireMissile", FireDelay, FireDelay);
		_previousTargetPosition = AttackTarget.transform.position;
	}

	void Update ()
	{
		var helicopterToTargetOffset = (_offset + AttackTarget.transform.position) - transform.position;
		helicopterToTargetOffset.y = 0;

		var velocity = Mathf.SmoothStep(0, FlySpeed, helicopterToTargetOffset.magnitude/MaxSpeedDistance);
		transform.position += helicopterToTargetOffset.normalized*velocity*Time.deltaTime;
		transform.LookAt(AttackTarget.transform.position);

		_targetMovement = (AttackTarget.transform.position - _previousTargetPosition)/Time.deltaTime;
		_previousTargetPosition = AttackTarget.transform.position;
	}

	private void SetNewRandomOffset()
	{
		var randomDirection = Quaternion.Euler(0, UnityEngine.Random.Range(-OffsetConeFromTarget, OffsetConeFromTarget), 0)*(-Vector3.forward);
		_offset = randomDirection*UnityEngine.Random.Range(TargetMinDistance, TargetMaxDistance);
	}

	[ContextMenu("Spawn Missile")]
	private void FireMissile()
	{
		if (_fireFromLeft)
		{
			SpawnMissile(MissileSpawnL);
		}
		else
		{
			SpawnMissile(MissileSpawnR);
		}

		_fireFromLeft = !_fireFromLeft;
	}

	private void SpawnMissile(Transform missileSpawn)
	{
		var missile = Instantiate(MissileTemplate);
		missile.transform.position = missileSpawn.position;
		missile.transform.rotation = transform.rotation;
		var missileMovement = missile.GetComponent<MissileMovement>();
		missileMovement.TargetPosition = AttackTarget.transform.position + new Vector3(0, 4, 0);
		missileMovement.TargetMovement = _targetMovement;
	}

    public void DoDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth < 0)
        {
            GetComponent<Animator>().SetTrigger("Crash");
            Destroy(this);
        }
    }
}
