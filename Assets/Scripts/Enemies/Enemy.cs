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
    public Vector3 relativeToConveyorBelt;          //Position that the enemy is relative to the conveyor belt.

	//Components
	private MeshRenderer[] mr;                      //Mesh Renderer components of all models attached to the enemy.

    //how much this takes off of the total budget for the wave
    public float budgetValue;

	//Effects
	public List<EnemyEffect> curEffects = new List<EnemyEffect>();
    private bool flashingColour = false;

    private void Awake()
    {
        sparkParticleEffect.Stop();
        smokeParticleEffect.Stop();
        deathParticleEffect.Stop();

        mr = GetComponentsInChildren<MeshRenderer>();
    }

    void Update ()
    {
		if(curEffects.Count > 0)
		{
			for(int index = 0; index < curEffects.Count; ++index)
			{
				curEffects[index].damageTimer += Time.deltaTime;
				curEffects[index].timer += Time.deltaTime;

				if(curEffects[index].damageTimer >= curEffects[index].rate)
				{
					TakeDamage(curEffects[index].damage, DamageType.Acid);
					curEffects[index].damageTimer = 0.0f;
				}

				if(curEffects[index].timer >= curEffects[index].duration)
				{
					curEffects.Remove(curEffects[index]);
					break;
				}
			}
		}

		//Move to position.
		if(positionToMoveTo != Vector3.zero)
			transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, 3.0f * Time.deltaTime);
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

        if(!flashingColour)
        {
            if (dmgType == DamageType.Acid)
            {
                StartCoroutine(Flash(Color.green));
            }
            else
            {
                StartCoroutine(Flash(Color.red));
            }
        }
    }

    IEnumerator Flash(Color col)
    {
        Color startColour = mr[0].material.color;

        flashingColour = true;

        for (int x = 0; x < mr.Length; ++x)
            mr[x].material.color = col;

        yield return new WaitForSeconds(0.05f);

        for (int x = 0; x < mr.Length; ++x)
            mr[x].material.color = startColour;

        flashingColour = false;
    }

 //   //Flash the enemy red.
 //   IEnumerator DamageFlash ()
	//{
	//	Color startColour = mr[0].material.color;

 //       flashingColour = true;

	//	for(int x = 0; x < mr.Length; ++x)
	//		mr[x].material.color = Color.red;
		
	//	yield return new WaitForSeconds(0.05f);

	//	for(int x = 0; x < mr.Length; ++x)
	//		mr[x].material.color = startColour;

 //       flashingColour = false;
	//}

 //   //Flash the enemy green.
	//IEnumerator PoisonFlash ()
	//{
	//	Color startColour = mr[0].material.color;

 //       flashingColour = true;

	//	for(int x = 0; x < mr.Length; ++x)
	//		mr[x].material.color = Color.green;

	//	yield return new WaitForSeconds(0.05f);

	//	for(int x = 0; x < mr.Length; ++x)
	//		mr[x].material.color = startColour;

 //       flashingColour = false;
	//}

    public void MoveOnPulse (ConveyorBelt nextConveyorBelt)
    {
		if(!curConveyorBelt.isFinalConveyorBelt)
		{
	        positionToMoveTo = curConveyorBelt.GetNextConveyorBeltPosition(relativeToConveyorBelt, this);
			SetRotation();
		}
		else
		{
			GetToEndOfPath();
		}
	}

	//Makes it so that the enemy is facing in the direction of the conveyor belt movement.
	void SetRotation ()
	{
		Vector3 dir = (positionToMoveTo - transform.position).normalized;
		float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

		transform.eulerAngles = new Vector3(0, angle - 90, 0);
	}

    //This function gets called when the enemy's health is less than or equals to 0.
    //It destroys the enemy, as well as removing it from lists.
    void Die (DamageType dmgType)
    {
        //Play and destroy the death particle effect.
        deathParticleEffect.transform.parent = null;
        deathParticleEffect.Play();
        Destroy(deathParticleEffect.gameObject, deathParticleEffect.main.duration);

        /*
        - Play death audio.
        */
        EnemyData death = new EnemyData();
        death.enemyClassData = this;
        death.finalBlowDamageType = dmgType;

        GlobalEvents.OnEnemyDeath.Invoke(death);
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnBotDeath();

        curConveyorBelt.curEnemies.Remove(this);
        
		GameManager.gm.AddScrap(ScrapValues.GetEnemyDropAmount(type));
        GameManager.gm.ui.CreateScrapIcon(type);

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
		GameManager.gm.health--;
		curConveyorBelt.curEnemies.Remove(this);
		CameraShake.cs.Shake(0.15f, 0.5f, 35.0f);
		Destroy(gameObject);
	}

    //This function gets called whent the enemy takes damage.
    //It checks to see if the health is below a certain amount and plays damage particle effects if so.
    void CheckDamageParticleEffects ()
    {
        //if the enemy's health is below 50%, then start to emit the spark particle effect.
        if (curHealth / (float)maxHealth <= 0.5f)
        {
            sparkParticleEffect.Play();
        }

        //If the enemy's health is below 20%, then start to emit the smoke particle effect.
        if (curHealth / (float)maxHealth <= 0.2f)
        {
            smokeParticleEffect.Play();
        }
    }

	public void ApplyAcidEffect (float duration, int damage, float rate)
	{
		curEffects.Add(new EnemyEffect(duration, damage, rate, EnemyEffectType.Acid));
	}
}

public enum EnemyType { Basic, TowerAttraction, EMP, Quick, MoltenMetal }
