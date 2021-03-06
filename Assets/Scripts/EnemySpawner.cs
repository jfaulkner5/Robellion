﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Calculations
{
    [Header("(((BV + W) * e) + AEA+ (ET * ETM))")]
    public float baseValue = 1;
    public float multiplier = 1;
    public float afterMultiplicationAddition;

    [Header("enemy type: 0 for all enemys equal")]
    public float enemyTypeMultiplier = 0;

    [Header("For display purposes only:")]
    public float calculatedValue;

    //for enemy type value check enum 
    public virtual float Calculate(int wave, int enemyType)
    {
        calculatedValue = (((baseValue + wave)) * multiplier) + afterMultiplicationAddition + (enemyType * enemyTypeMultiplier);
        return calculatedValue;
    }
}

[System.Serializable]
public class WaveData
{
    public int waveNumber;
    public int numEnemiesPerConveyor;
    public List<GameObject> enemies = new List<GameObject>();
    public List<float> resistPercentages = new List<float>();
}

[System.Serializable]
public class AConveyor
{
    public List<GameObject> enemies = new List<GameObject>();
    [Header("-0.2 or 0.2 for x and z")]
    public List<Vector3> ConveyorLocation = new List<Vector3>();
}

[System.Serializable]
public class AWave
{
    public List<AConveyor> conveyors = new List<AConveyor>();
}

public class EnemySpawner : MonoBehaviour 
{
    public Transform enemyParentObject;

	public GameObject[] enemyPrefab;
	public ConveyorBelt spawningBelt;
    public int[] bossWaves;
    public Calculations budget;

    [Header("count equivalent to # of damage types")]
    public float[] deathsInPrevWave = {0, 0, 0, 0, 0};
    public int totalDeathsInPrevWave = 0;

    public Calculations HealthStatCalcs;
    public Calculations DamageResistStatCalcs;

    public WaveData currentWave;

    public Animator animateLeft;
    public Animator animateRight;

    public List<AWave> waves = new List<AWave>();

    public void Awake()
    {
        GlobalEvents.OnEnemyDeath.AddListener(OnEnemyDeath);
        GlobalEvents.OnPulse.AddListener(SpawnEnemiesOnPulse);
    }

    public void SpawnEnemies (int wave)
	{
		float rate = Mathf.Clamp(1.0f + (0.1f * wave), 1f, 4f);
        List<GameObject> enemies = new List<GameObject>();

        float budgetForWave = budget.Calculate(wave, 0);
        
        while(budgetForWave > 0)
        {
            int index = Random.Range(0, enemyPrefab.Length-1);
            enemies.Add(enemyPrefab[index]);
            budgetForWave -= enemyPrefab[index].GetComponent<Enemy>().budgetValue;
        }

        currentWave.waveNumber = wave;
        currentWave.enemies = enemies;
        currentWave.resistPercentages = CalculateDeathPercentages();
        currentWave.numEnemiesPerConveyor = (int)rate;
		//StartCoroutine(SpawnEnemiesTimer(enemies, CalculateDeathPercentages(), wave, rate));
	}

    public List<float> CalculateDeathPercentages()
    {
        List<float> temp = new List<float>();

        for (int index = 0; index < deathsInPrevWave.Length; ++index)
        {
            temp.Add(deathsInPrevWave[index] / totalDeathsInPrevWave);

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

    private bool oneExtraOpen = false;
    public void SpawnEnemiesOnPulse (PulseData pd)
    {
        if (currentWave.enemies.Count == 0 && oneExtraOpen)
        {
            StartCoroutine(Play());
            oneExtraOpen = false;
        }

        if (currentWave.enemies.Count > 0)
        {
            StartCoroutine(Play());
            oneExtraOpen = true;
            if(GameManager.gm.ui.isSurvival)
            {
                List<Vector3> offsets = new List<Vector3>();
                offsets.Add(new Vector3(-0.2f, 0, -0.2f));
                offsets.Add(new Vector3(-0.2f, 0, 0.2f));
                offsets.Add(new Vector3(0.2f, 0, -0.2f));
                offsets.Add(new Vector3(0.2f, 0, 0.2f));

                for (int i = 0; i < Random.Range(0, 4); ++i)
                {
                    if (currentWave.enemies.Count > 0)
                    {
                        int num = Random.Range(0, offsets.Count);
                        CreateEnemy(currentWave.enemies[0], offsets[num]);
                        offsets.RemoveAt(num);
                    }
                }
            }
            else
            {
                if (waves[currentWave.waveNumber - 1].conveyors.Count > 0)
                {
                    for (int x = 0; x < waves[currentWave.waveNumber - 1].conveyors[0].enemies.Count; ++x)
                    {
                        CreateEnemy(waves[currentWave.waveNumber - 1].conveyors[0].enemies[x], waves[currentWave.waveNumber - 1].conveyors[0].ConveyorLocation[x]);
                    }

                    waves[currentWave.waveNumber - 1].conveyors.RemoveAt(0);
                }
            }
        }
    }
  
    public Enemy CreateEnemy(GameObject e, Vector3 offset)
    {
        //Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));

        //if (offset.x > 0.0f) { offset.x = 0.2f; }
        //else { offset.x = -0.2f; }

        //if (offset.z > 0.0f) { offset.z = 0.2f; }
        //else { offset.z = -0.2f; }

        //Instantiate the enemy.
        GameObject enemy = Instantiate(e, transform.position + offset, Quaternion.identity, enemyParentObject);

        if(GameManager.gm.ui.isSurvival)
            currentWave.enemies.RemoveAt(0);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.horizontalOffsetOnConveyorBelt = offset.z;
        //Set values.
        enemyScript.relativeToConveyorBelt = offset;
        enemyScript.curConveyorBelt = spawningBelt;

        enemyScript.maxHealth = (int)HealthStatCalcs.Calculate(currentWave.waveNumber, (int)enemyScript.type);
        enemyScript.curHealth = enemyScript.maxHealth;

        float randomNumber = Random.value;
        float resistanceVal = 0;

        for (int i = 0; i < currentWave.resistPercentages.Count; ++i)
        {
            resistanceVal += currentWave.resistPercentages[i];

            if (randomNumber < resistanceVal)
            {
                enemyScript.resistType = (DamageType)i;
                break;
            }
        }

        enemyScript.resistValue = (int)Mathf.Clamp(DamageResistStatCalcs.Calculate(currentWave.waveNumber, (int)enemyScript.type), 0, 100);

        return enemyScript;
    }

    //void OnEnable()
    //{
    //    GlobalEvents.OnPulse.AddListener(OnPulse);
    //}

    //void OnDisable()
    //{
    //    GlobalEvents.OnPulse.RemoveListener(OnPulse);
    //}

    IEnumerator Play()
    {
        animateLeft.Play("SpawnerLeftDoor");
        animateRight.Play("SpawnerRightDoor");

        yield return new WaitForSeconds(.5f);

        animateLeft.Play("Idle");
        animateRight.Play("Idle");
    }

    
    //public virtual void OnPulse(PulseData pd)
    //{
        

    //}

    //IEnumerator SpawnEnemiesTimer (List<GameObject> enemies, float[] resistancePercentages, int waveNum,  float rate)
    //{
    //	for(int index = 0; index < enemies.Count; ++index)
    //	{
    //           Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));

    //           if(offset.x > 0.0f) { offset.x = 0.2f; }
    //           else { offset.x = -0.2f; }

    //           if(offset.z > 0.0f) { offset.z = 0.2f; }
    //           else { offset.z = -0.2f; }

    //           //Instantiate the enemy.
    //           GameObject enemy = Instantiate(enemies[index], transform.position + offset, Quaternion.identity, enemyParentObject);
    //		Enemy enemyScript = enemy.GetComponent<Enemy>();

    //           //Set values.
    //           enemyScript.relativeToConveyorBelt = offset;
    //		enemyScript.curConveyorBelt = spawningBelt;

    //           enemyScript.maxHealth = (int)HealthStatCalcs.Calculate(waveNum, (int)enemyScript.type);
    //           enemyScript.curHealth = enemyScript.maxHealth;

    //           float randomNumber = Random.value;
    //           float resistanceVal = 0; 

    //           for(int i = 0; i < resistancePercentages.Length; ++i)
    //           {
    //               resistanceVal += resistancePercentages[i];

    //               if (randomNumber < resistanceVal)
    //               {
    //                   enemyScript.resistType = (DamageType)i;
    //                   break;
    //               }
    //           }

    //           enemyScript.resistValue = (int)Mathf.Clamp(DamageResistStatCalcs.Calculate(waveNum, (int)enemyScript.type), 0, 100);

    //           GameManager.gm.enemies.Add(enemyScript);

    //		yield return new WaitForSeconds(rate);
    //	}
    //}
}

