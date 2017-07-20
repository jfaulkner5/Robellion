using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject[] enemyPrefab;
	public ConveyorBelt spawningBelt;
	
	public void SpawnEnemies (int wave)
	{
		int amount = 3 + (wave * 3);
		float rate = 1.0f;

		StartCoroutine(SpawnEnemiesTimer(amount, Mathf.Clamp(rate - (0.01f * wave),0.3f,rate)));
	}

	IEnumerator SpawnEnemiesTimer (int amount, float rate)
	{
		for(int x = 0; x < amount; x++)
		{
            float horizOffset = Random.Range(-0.2f, 0.2f);
            GameObject enemy = Instantiate(enemyPrefab[0], transform.position + new Vector3(0, 0, horizOffset), Quaternion.identity);

			Enemy enemyScript = enemy.GetComponent<Enemy>();

            enemyScript.horizontalOffsetOnConveyorBelt = horizOffset;
			enemyScript.curConveyorBelt = spawningBelt;

			GameManager.gm.enemies.Add(enemyScript);

			yield return new WaitForSeconds(rate);
		}
	}
}

