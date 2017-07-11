using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int curHealth;
    public int maxHealth;

    //Particle Effects
    public ParticleSystem sparkParticleEffect;
    public ParticleSystem smokeParticleEffect;
    public ParticleSystem deathParticleEffect;

    //Conveyor Belt
    public ConveyorBelt curConveyorBelt;
    private Vector3 positionToMoveTo;           //The position in the middle of the next conveyor belt.
    public float horizontalOffsetOnConveyorBelt;    //The horizontal offset of the enemy on the conveyor belt.

    void Update ()
    {
        //Move enemy to next position on the next conveyor belt.
        if(Vector3.Distance(transform.position, positionToMoveTo) < 0.05f)
        {
            positionToMoveTo = curConveyorBelt.GetNextConveyorBeltPosition(horizontalOffsetOnConveyorBelt);
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
        if(curHealth - damageTaken <= 0)
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
        - Remove from lists.
        - Play death audio.
        - If enemy type is molten metal, then call function is MoltenMetalRobot.cs.
        */

        Destroy(gameObject);
    }

    //This function gets called whent the enemy takes damage.
    //It checks to see if the health is below a certain amount and plays damage particle effects if so.
    void CheckDamageParticleEffects ()
    {
        //Check to see if we should start to emit the spark particle effect.
        if(curHealth / maxHealth <= 0.5f)
        {
            sparkParticleEffect.Play();
        }

        //Check to see if we should start to emit the smoke particle effect.
        if(curHealth / maxHealth <= 0.2f)
        {
            smokeParticleEffect.Play();
        }
    }
}
