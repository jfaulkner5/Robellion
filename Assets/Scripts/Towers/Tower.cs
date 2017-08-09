using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum DamageType
{
    Melee,
    Heat,
    Electricity,
    Acid,
    Piercing
}

/// <summary>
/// Manages tower data, tower damage, target, attacking.
/// </summary>
public class Tower : MonoBehaviour 
{
    public TowerType type;
    public DamageType damType;

    //Range In Game Tiles min 1 max 3
    [Range(1,3)] public int range;

    //public AttackData attack;
    public int damage;
    public int pulsesBetweenAttacks;
    private int pulseTimer;

    //Enemies Within Range must be sorted by closest to the exit
    public List<Enemy> enemiesWithinRange = new List<Enemy>();
    
    public Enemy target = null;

	public bool canAttack = true;
	public bool rotateToTarget;
    protected Vector3 lookTarget;
    protected Vector3 baseLookTarget;


    public TowerPlatform towerPlatform; //Tower platform that this tower is on.

	public MeshRenderer[] mr;   //Array of all model mesh renderers.

    void OnEnable()
    {
        GlobalEvents.OnPulse.AddListener(OnPulse);
        GlobalEvents.OnAlternatePulse.AddListener(OnAlternatePulse);
    }

    void OnDisable()
    {
        GlobalEvents.OnPulse.RemoveListener(OnPulse);
        GlobalEvents.OnAlternatePulse.RemoveListener(OnAlternatePulse);
    }

    void Start()
    {
        GlobalEvents.OnEnemyDeath.AddListener(RemoveEnemyFromRange);
    }

    public void setLookTargets(Vector3 look)
    {
        baseLookTarget = look;
        lookTarget = look;
    }

    void Update () 
	{
        if(rotateToTarget && lookTarget != Vector3.zero)
            RotateToTarget(lookTarget);
	}

    public virtual void OnPulse(PulseData pd)
    {
        lookTarget = baseLookTarget;
    }

    public virtual void OnAlternatePulse(PulseData pd)
    {
        pulseTimer++;
        if (CanAttack())
        {
            GetTarget();
            if (target != null)
            {
                if (rotateToTarget)
                    lookTarget = target.transform.position;

                Attack();
            }
        }
        else
        {
            Reloading();
        }
    }

    protected virtual void Reloading()
    {
        
    }

    protected void RotateToTarget(Vector3 t)
    {
        Vector3 lookPos = t - transform.position;
        lookPos.y = 0;

        Quaternion rot = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5.0f);
    }


    private void GetTarget()
    {
        if (!enemiesWithinRange.Contains(target))
        {
            target = null;
        }

        for (int index = 0; index < enemiesWithinRange.Count; ++index)
        {
            if (enemiesWithinRange[index].type == EnemyType.TowerAttraction)
            {
                target = enemiesWithinRange[index];
            }
        }

        if (target == null && enemiesWithinRange.Count > 0)
        {
            target = enemiesWithinRange[0];
        }
    }


    public void AddEnemyToRange (EnemyData enemyInRange)
    {
        enemiesWithinRange.Add(enemyInRange.enemyClassData);
    }

    public void RemoveEnemyFromRange (EnemyData enemyOutOfRange)
    {
        enemiesWithinRange.Remove(enemyOutOfRange.enemyClassData);
    }

    protected virtual bool CanAttack()
    {
        return pulseTimer >= pulsesBetweenAttacks;
    }

    protected void Attack ()
    {
		if(canAttack && CanAttack())
        {
            PlayAttackSound();
            AttackAnimation();

            AttackDamage();
            pulseTimer = 0;
        }
    }

    protected virtual void AttackAnimation()
    {

    }

    protected virtual void AttackDamage()
    {
        target.TakeDamage(damage, damType);
    }

    protected virtual void PlayAttackSound()
    {

    }

	public void Sell ()
	{
		GameManager.gm.towers.Remove(this);
		GameManager.gm.AddScrap(50);
		towerPlatform.hasTower = false;
		Destroy(gameObject);
	}

	public void Upgrade ()
	{
		
	}

	public void EMPDisable (float disableTime)
	{
		canAttack = false;

		for(int x = 0; x < mr.Length; ++x)
		{
			mr[x].material.color = Color.blue;
		}

		StartCoroutine(EMPDisableTimer(disableTime));
	}

	IEnumerator EMPDisableTimer (float disableTime)
	{
		yield return new WaitForSeconds(disableTime);
		canAttack = true;

		for(int x = 0; x < mr.Length; ++x)
		{
			mr[x].material.color = Color.white;
		}
	}
}

public enum TowerType { RobotArm = 0, Crusher = 1, Lazer = 2, AcidEtcher = 3, Drill = 4 }

///// <summary>
///// 
///// </summary>
//[System.Serializable]
//public class AttackData
//{
//    public AnimationClip attackAnimation;
//    public float timeBetweenAttacks;

//    public int damageMin;
//    public int damageMax;

//    //Particle Effects
//    public ParticleSystem attackParticleEffect;

//    public float timer = 1000;

//    public bool canAttack()
//    {
//        float totalTime = timeBetweenAttacks;

//        if(attackAnimation != null)
//        {
//            totalTime += attackAnimation.length;
//        }

//        if(timer > totalTime)
//        {
//            timer = 0;
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//}