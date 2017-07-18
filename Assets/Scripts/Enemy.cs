using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy data, damage taken, death and movement.
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyType type;

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

	void Start ()
	{
		positionToMoveTo = curConveyorBelt.GetNextConveyorBeltPosition(horizontalOffsetOnConveyorBelt);
	}

    void Update ()
    {
        //If the enemy is at the positionToMoveTo, then get the next position. Otherwise, move the enemy to that position.
        if (Vector3.Distance(transform.position, positionToMoveTo) < 0.01f)
        {
            positionToMoveTo = curConveyorBelt.GetNextConveyorBeltPosition(horizontalOffsetOnConveyorBelt);
			transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, curConveyorBelt.speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, curConveyorBelt.speed * Time.deltaTime);
        }
    }

    //This function gets called when the enemy takes damage.
    //damageTaken, is the amount of damage that needs to be taken.
    public void TakeDamage (int damageTaken)
    {
        if (curHealth - damageTaken <= 0)
        {
            Die();
        }
        else
        {
            curHealth -= damageTaken;
            CheckDamageParticleEffects();
        }
    }

    //This function gets called when the enemy's health is less than or equals to 0.
    //It destroys the enemy, as well as removing it from lists.
    void Die ()
    {
        //Play and destroy the death particle effect.
        deathParticleEffect.transform.parent = null;
        deathParticleEffect.Play();
        Destroy(deathParticleEffect.gameObject, deathParticleEffect.main.duration);

        /*
        - Remove from GameManager lists.
        - Play death audio.
        */

        if(type == EnemyType.MoltenMetal)
        {
            //Call explode function.
        }

        Destroy(gameObject);
    }

    //This function gets called whent the enemy takes damage.
    //It checks to see if the health is below a certain amount and plays damage particle effects if so.
    void CheckDamageParticleEffects ()
    {
        //if the enemy's health is below 50%, then start to emit the spark particle effect.
        if (curHealth / maxHealth <= 0.5f)
        {
            sparkParticleEffect.Play();
        }

        //If the enemy's health is below 20%, then start to emit the smoke particle effect.
        if (curHealth / maxHealth <= 0.2f)
        {
            smokeParticleEffect.Play();
        }
    }
}

public enum EnemyType { Basic, TowerAttraction, EMP, Quick, MoltenMetal }
