using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tower data, tower damage, target, attacking.
/// </summary>
public class Tower : MonoBehaviour {

    public TowerType type;

    //Range In Game Tiles min 1 max 3
    [Range(1,3)]public int range;

    public AttackData attack;

    //Enemies Within Range must be sorted by closest to the exit
    public List<Enemy> enemiesWithinRange = new List<Enemy>();
    
    public Enemy target = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //enemiesWithinRange.RemoveAll(null);
        attack.timer += Time.deltaTime;

        if (!enemiesWithinRange.Contains(target))
        {
            target = null;
        }

        for (int index = 0; index < enemiesWithinRange.Count; ++index)
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
        }
	}

    public void AddEnemyToRange(Enemy enemyInRange)
    {
        enemiesWithinRange.Add(enemyInRange);
    }

    public void RemoveEnemyFromRange(Enemy enemyOutOfRange)
    {
        enemiesWithinRange.Remove(enemyOutOfRange);
    }

    protected virtual void Attack(Enemy target)
    {
        if(attack.canAttack())
        {
            target.TakeDamage(1);//Random.Range(attack.damageMin, attack.damageMax));
        }
    }
}

public enum TowerType { RobotArm, Crusher, Lazer, AcidEtcher}

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