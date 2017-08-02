using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy data, damage taken, death and movement.
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyType type;

    public DamageType resistType;
    [Range(0, 100)] public int resistValue;

    public int curHealth;
    public int maxHealth;

    //Particle Effects
    public ParticleSystem sparkParticleEffect;
    public ParticleSystem smokeParticleEffect;
    public ParticleSystem deathParticleEffect;

    //Conveyor Belt
    public ConveyorBelt curConveyorBelt;
    private Vector3 positionToMoveTo;               //The position in the middle of the next conveyor belt.
    public float horizontalOffsetOnConveyorBelt;    //The horizontal offset of the enemy on the conveyor belt.

	//Components
	public GameObject model;
	public MeshRenderer[] mr;


    //how much this takes off of the total budget for the wave
    public float budgetValue;

	//Effects
	public List<EnemyEffect> curEffects = new List<EnemyEffect>();

    void Start ()
	{
		positionToMoveTo = curConveyorBelt.GetNextConveyorBeltPosition(horizontalOffsetOnConveyorBelt);
	}

    void Update ()
    {
        //If the enemy is at the positionToMoveTo, then get the next position. Otherwise, move the enemy to that position.
        if(Vector3.Distance(transform.position, positionToMoveTo) < 0.01f)
        {
			if(!curConveyorBelt.nextConveyorBelt)
			{
				GetToEndOfPath();
			}
			else
			{
            	positionToMoveTo = curConveyorBelt.GetNextConveyorBeltPosition(horizontalOffsetOnConveyorBelt);
				transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, curConveyorBelt.speed * Time.deltaTime);
			}
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, curConveyorBelt.speed * Time.deltaTime);
        }

		if(curEffects.Count > 0)
		{
			for(int x = 0; x < curEffects.Count; ++x)
			{
				curEffects[x].damageTimer += Time.deltaTime;
				curEffects[x].timer += Time.deltaTime;

				if(curEffects[x].damageTimer >= curEffects[x].rate)
				{
					TakeDamage(curEffects[x].damage, DamageType.Acid);
					curEffects[x].damageTimer = 0.0f;
				}

				if(curEffects[x].timer >= curEffects[x].duration)
				{
					curEffects.Remove(curEffects[x]);
					break;
				}
			}
		}
    }

    //This function gets called when the enemy takes damage.
    //damageTaken, is the amount of damage that needs to be taken.
    public void TakeDamage (int damageTaken, DamageType dmgType)
    {
        float damTaken = damageTaken;
        if(dmgType == resistType)
        {
            damTaken -= damageTaken / (float)resistValue;
        }

        if(curHealth - (int)damTaken <= 0)
        {
            Die(dmgType);
        }
        else
        {
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnBotDamage();
            curHealth -= (int)damTaken;
            CheckDamageParticleEffects();
        }

		if(dmgType == DamageType.Acid)
		{
			StartCoroutine(PoisonFlash());
		}
		else
		{
			StartCoroutine(DamageFlash());
		}
    }

	IEnumerator DamageFlash ()
	{
		Color startColour = mr[0].material.color;

		for(int x = 0; x < mr.Length; ++x)
			mr[x].material.color = Color.red;
		
		yield return new WaitForSeconds(0.05f);

		for(int x = 0; x < mr.Length; ++x)
			mr[x].material.color = startColour;
	}

	IEnumerator PoisonFlash ()
	{
		Color startColour = mr[0].material.color;

		for(int x = 0; x < mr.Length; ++x)
			mr[x].material.color = Color.green;

		yield return new WaitForSeconds(0.05f);

		for(int x = 0; x < mr.Length; ++x)
			mr[x].material.color = startColour;
	}

    //This function gets called when the enemy's health is less than or equals to 0.
    //It destroys the enemy, as well as removing it from lists.
    void Die (DamageType dmgType)
    {
        //Play and destroy the death particle effect.
        /*deathParticleEffect.transform.parent = null;
        deathParticleEffect.Play();
        Destroy(deathParticleEffect.gameObject, deathParticleEffect.main.duration);*/

        /*
        - Play death audio.
        */
        EnemyData death = new EnemyData();
        death.enemyClassData = this;
        death.finalBlowDamageType = dmgType;

        GlobalEvents.OnEnemyDeath.Invoke(death);
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnBotDeath();

		GameManager.gm.enemies.Remove(this);
		GameManager.gm.AddScrap(20);

		//If the enemy is molten metal, then do what it does.
    //    if(type == EnemyType.MoltenMetal)
    //    {
    //        if(GetComponent<MoltenMetalRobot>().isHot)
    //        {
				//GetComponent<MoltenMetalRobot>().DamageNearbyEnemies();
    //        }
    //    }

        Destroy(gameObject);
    }

	public void GetToEndOfPath ()
	{
		GameManager.gm.enemies.Remove(this);
		GameManager.gm.health--;
		CameraShake.cs.Shake(0.15f, 0.1f, 35.0f);
		Destroy(gameObject);
	}

    //This function gets called whent the enemy takes damage.
    //It checks to see if the health is below a certain amount and plays damage particle effects if so.
    void CheckDamageParticleEffects ()
    {
        //if the enemy's health is below 50%, then start to emit the spark particle effect.
        if (curHealth / maxHealth <= 0.5f)
        {
            //sparkParticleEffect.Play();
        }

        //If the enemy's health is below 20%, then start to emit the smoke particle effect.
        if (curHealth / maxHealth <= 0.2f)
        {
            //smokeParticleEffect.Play();
        }
    }

	public void ApplyAcidEffect (float duration, int damage, float rate)
	{
		curEffects.Add(new EnemyEffect(duration, damage, rate, EnemyEffectType.Acid));
	}
}

public enum EnemyType { Basic, TowerAttraction, EMP, Quick, MoltenMetal }
