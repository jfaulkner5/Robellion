using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BudgetCalculations
{
    [Header("((X*wVM)+wVA)^e")]
    public float waveValueMultiplier;
    public float exponent;
    public float waveValueAdditive;

    [Header("For display purposes only:")]
    public float calculatedBudget;

    public float CalculateBudget(int wave)
    {
        calculatedBudget = Mathf.Pow(((wave * waveValueMultiplier) + waveValueAdditive), exponent);
        return calculatedBudget;
    }
}

[System.Serializable]
public class EnemyStatCalculations
{
    [Header("(((BV + W + (ET * ETM)) + BEA) ^ e) + AEA")]
    public float baseValue;
    public float beforeExponentAddition;
    public float exponent;
    public float afterExponentAddition;

    [Header("enemy type: 0 for all enemys equal")]
    public float enemyTypeMultiplier;
    
    //for enemy type value check enum 
    public int Calculate(int wave, int enemyType)
    {
        return (int)(Mathf.Pow(((baseValue +(wave + (enemyType * enemyTypeMultiplier))) + beforeExponentAddition), exponent) + afterExponentAddition);
    }
}

public class EnemySpawner : MonoBehaviour 
{
    public Transform enemyParentObject;

	public GameObject[] enemyPrefab;
	public ConveyorBelt spawningBelt;
    public int[] bossWaves;
    public BudgetCalculations budget;

    [Header("count equivalent to # of damage types")]
    public float[] deathsInPrevWave = {0, 0, 0, 0};
    public int totalDeathsInPrevWave;

    public EnemyStatCalculations HealthStatCalcs;
    public EnemyStatCalculations DamageResistStatCalcs;

    public void Awake()
    {
        GlobalEvents.OnEnemyDeath.AddListener(OnEnemyDeath);
    }

    public void SpawnEnemies (int wave)
	{
		float rate = Mathf.Clamp(1.0f - (0.01f * wave), 0.3f, 1.0f);
        List<GameObject> enemies = new List<GameObject>();

        float budgetForWave = budget.CalculateBudget(wave);
        
        while(budgetForWave > 0)
        {
            int index = Random.Range(0, enemyPrefab.Length-1);
            enemies.Add(enemyPrefab[index]);
            budgetForWave -= enemyPrefab[index].GetComponent<Enemy>().budgetValue;
        }
        
		StartCoroutine(SpawnEnemiesTimer(enemies, CalculateDeathPercentages(), wave, rate));
	}

    public float[] CalculateDeathPercentages()
    {
        float[] temp = deathsInPrevWave;

        for (int index = 0; index < temp.Length; ++index)
        {
            temp[index] /= totalDeathsInPrevWave;

            //reset deaths
            deathsInPrevWave[index] = 0;
        }

        totalDeathsInPrevWave = 0;
        return temp;
    }

    //event to add to death counts
    public void OnEnemyDeath(EnemyData deadEnemy)
    {
        totalDeathsInPrevWave++;
        deathsInPrevWave[(int)deadEnemy.finalBlowDamageType]++;
    }

	IEnumerator SpawnEnemiesTimer (List<GameObject> enemies, float[] resistancePercentages, int waveNum,  float rate)
	{
		for(int index = 0; index < enemies.Count; ++index)
		{
            float horizOffset = 0;

            if(Random.value > 0.5f)
            {
                horizOffset = -0.2f;
            }
            else
            {
                horizOffset = 0.2f;
            }

            //Instantiate the enemy.
            GameObject enemy = Instantiate(enemies[index], transform.position + new Vector3(0, 0, horizOffset), Quaternion.identity, enemyParentObject);
			Enemy enemyScript = enemy.GetComponent<Enemy>();

            //Set values.
            enemyScript.horizontalOffsetOnConveyorBelt = horizOffset;
			enemyScript.curConveyorBelt = spawningBelt;

            enemyScript.maxHealth = HealthStatCalcs.Calculate(waveNum, (int)enemyScript.type);
            enemyScript.curHealth = enemyScript.maxHealth;

            float randomNumber = Random.value;
            float resistanceVal = Mathf.Clamp(DamageResistStatCalcs.Calculate(waveNum, (int)enemyScript.type),0,100);

            for(int i = 0; i < resistancePercentages.Length; ++i)
            {
                resistanceVal += resistancePercentages[i];

                if (randomNumber < resistanceVal)
                {
                    enemyScript.resistType = (DamageType)i;
                    break;
                }
            }
            
			GameManager.gm.enemies.Add(enemyScript);

			yield return new WaitForSeconds(rate);
		}
	}
}

