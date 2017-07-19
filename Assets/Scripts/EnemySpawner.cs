using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject enemyPrefab;
	public ConveyorBelt spawningBelt;
	
	public void SpawnEnemies (int wave)
	{
		int amount = 3 + (wave * 2);
		float rate = 1.0f;

		StartCoroutine(SpawnEnemiesTimer(amount, rate));
	}

	IEnumerator SpawnEnemiesTimer (int amount, float rate)
	{
		for(int x = 0; x < amount; x++)
		{
			GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

			Enemy enemyScript = enemy.GetComponent<Enemy>();

			enemyScript.horizontalOffsetOnConveyorBelt = Random.Range(-0.2f, 0.2f);
			enemyScript.curConveyorBelt = spawningBelt;

			GameManager.gm.enemies.Add(enemyScript);

			yield return new WaitForSeconds(rate);
		}
	}
}

