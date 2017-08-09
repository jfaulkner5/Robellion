using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyEffect 
{
	public EnemyEffectType type;

	public float duration;
	public int damage;
	public float rate;

	public float timer = 0.0f;
	public float damageTimer = 0.0f;

	public EnemyEffect (float duration, int damage, float rate, EnemyEffectType type)
	{
		this.duration = duration;
		this.damage = damage;
		this.rate = rate;
		this.type = type;
	}
}

public enum EnemyEffectType { Acid }
