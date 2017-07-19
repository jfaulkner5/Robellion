using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlatform : MonoBehaviour 
{
	public bool hasTower;
	public GameObject towerPrefab;

	public void BuildTower (TowerType towerType)
	{
		//DEBUG TESTING
		if(!hasTower)
		{
			if(GameManager.gm.curScrap >= GameManager.gm.basicTowerCost)
			{
				GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
				hasTower = true;

				GameManager.gm.curScrap -= GameManager.gm.basicTowerCost;
			}
		}
	}
}
