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

    public float CalculateBudget(int wave)
    {
        return Mathf.Pow(((wave * waveValueMultiplier) + waveValueAdditive),exponent);
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
        
		StartCoroutine(SpawnEnemiesTimer(enemies, CalculateDeathPercentages(), rate));
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
    public void OnEnemyDeath(Enemy e, int dmgType)
    {
        totalDeathsInPrevWave++;
        deathsInPrevWave[dmgType]++;
    }

	IEnumerator SpawnEnemiesTimer (List<GameObject> enemies, float[] resistancePercentages, float rate)
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
            enemyScript.type = EnemyType.Basic;

            enemyScript.OnEnemyDeath.AddListener(OnEnemyDeath);

            float randomNumber = Random.value;
            float resistanceVal = 0;

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

