﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tower data, tower damage, target, attacking.
/// </summary>
public class Tower : MonoBehaviour 
{
    public TowerType type;

    //Range In Game Tiles min 1 max 3
    [Range(1,3)] public int range;

    public AttackData attack;

    //Enemies Within Range must be sorted by closest to the exit
    public List<Enemy> enemiesWithinRange = new List<Enemy>();
    
    public Enemy target = null;

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

		//REALLY BAD THING FOR THE CLIENT MEETING
		//JUST TRYING TO MAKE THE TOWER WORK
		for(int x = 0; x < GameManager.gm.enemies.Count; x++)
		{
			if(Vector3.Distance(transform.position, GameManager.gm.enemies[x].transform.position) <= range)
			{
				enemiesWithinRange.Add(GameManager.gm.enemies[x]);
			}
		}

		for(int x = 0; x < enemiesWithinRange.Count; x++)
		{
			if(enemiesWithinRange[x] == null)
			{
				enemiesWithinRange.Remove(enemiesWithinRange[x]);
				continue;
			}

			if(Vector3.Distance(transform.position, enemiesWithinRange[x].transform.position) > range)
			{
				enemiesWithinRange.Remove(enemiesWithinRange[x]);
			}
		}

		if (target != null)
        {
            Attack(target);
        }
	}

    public void AddEnemyToRange (Enemy enemyInRange)
    {
        enemiesWithinRange.Add(enemyInRange);
    }

    public void RemoveEnemyFromRange (Enemy enemyOutOfRange)
    {
        enemiesWithinRange.Remove(enemyOutOfRange);
    }

    protected virtual void Attack (Enemy target)
    {
        if(attack.canAttack())
        {
            target.TakeDamage(1);	//Random.Range(attack.damageMin, attack.damageMax));
        }
    }
}

public enum TowerType { RobotArm, Crusher, Lazer, AcidEtcher }

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