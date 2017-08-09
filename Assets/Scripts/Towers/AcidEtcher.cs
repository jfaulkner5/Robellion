using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEtcher : MonoBehaviour 
{
	public Tower tower;		//Base tower parent.

	public float attackRate;
	private float attackTimer;

	public ParticleSystem acidEffect;

	void Update ()
	{
		attackTimer += Time.deltaTime;

		if(tower.enemiesWithinRange.Count > 0)
		{
			if(attackTimer >= attackRate)
			{
				PoisonEnemies();
				attackTimer = 0.0f;
			}

			if(!acidEffect.isPlaying)
				acidEffect.Play();
		}
		else
		{
			if(acidEffect.isPlaying)
				acidEffect.Stop();
		}
	}

	void PoisonEnemies ()
	{
		for(int x = 0; x < tower.enemiesWithinRange.Count; ++x)
		{
			if(tower.enemiesWithinRange[x].curEffects.Count == 0)
			{
				tower.enemiesWithinRange[x].ApplyAcidEffect(3.0f, 1, 1.0f);
			}
		}
	}
}
