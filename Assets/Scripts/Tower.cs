﻿using System.Collections;
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

    public AttackData attack;

    //Enemies Within Range must be sorted by closest to the exit
    public List<Enemy> enemiesWithinRange = new List<Enemy>();
    
    public Enemy target = null;

	public bool canAttack = true;

    public LineRenderer lr;

	public TowerPlatform towerPlatform; //Tower platform that this tower is on.

	public MeshRenderer[] mr;	//Array of all model mesh renderers.

    private void Start()
    {
        lr.SetPosition(0, transform.position + new Vector3(0, 0.5f, 0));
    }

	void Update () 
	{
        //enemiesWithinRange.RemoveAll(null);
        attack.timer += Time.deltaTime;

        if(!enemiesWithinRange.Contains(target))
        {
            target = null;
        }

        for(int index = 0; index < enemiesWithinRange.Count; ++index)
        {
            if(enemiesWithinRange[index].type == EnemyType.TowerAttraction)
            {
                target = enemiesWithinRange[index];
            }
        }

        if(target == null && enemiesWithinRange.Count > 0)
        {
            target = enemiesWithinRange[0];
        }

        if (target != null)
        {
            Attack(target);
            lr.enabled = true;
            lr.SetPosition(1, target.transform.position);
        }
        else
        {
            lr.enabled = false;
        }
	}

    public void AddEnemyToRange (Enemy enemyInRange)
    {
        enemiesWithinRange.Add(enemyInRange);
        enemyInRange.OnEnemyDeath.AddListener(RemoveEnemyFromRange2);
    }

    //jerryrigged bc tired
    //wtf?
    public void RemoveEnemyFromRange2 (Enemy enemyOutOfRange, int unused)
    {
        enemiesWithinRange.Remove(enemyOutOfRange);
    }
    public void RemoveEnemyFromRange (Enemy enemyOutOfRange)
    {
        enemiesWithinRange.Remove(enemyOutOfRange);
    }

    protected virtual void Attack (Enemy target)
    {
        if(attack.canAttack())
        {
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackLazer();
            target.TakeDamage(1, damType);	//Random.Range(attack.damageMin, attack.damageMax));
        }
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

public enum TowerType { RobotArm, Crusher, Lazer, AcidEtcher, Drill }

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class AttackData
{
    public AnimationClip attackAnimation;
    public float timeBetweenAttacks;

    public int damageMin;
    public int damageMax;

    //Particle Effects
    public ParticleSystem attackParticleEffect;

    public float timer = 1000;

    public bool canAttack()
    {
        float totalTime = timeBetweenAttacks;

        if(attackAnimation != null)
        {
            totalTime += attackAnimation.length;
        }

        if(timer > totalTime)
        {
            timer = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}