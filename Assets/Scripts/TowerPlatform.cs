using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlatform : MonoBehaviour 
{
	public bool hasTower;
	public GameObject towerPrefab;

    public ConveyorBelt firstConveyorInRange;
    public TowerPlatform oppositePlatform;

	public void BuildTower (TowerType towerType)
	{
		//DEBUG TESTING
		if(!hasTower)
		{
			if(GameManager.gm.curScrap >= GameManager.gm.basicTowerCost)
			{
				GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);

                Tower t = tower.GetComponent<Tower>();
                ConveyorBelt curConveyor = firstConveyorInRange;
				GameManager.gm.towers.Add(t);
				t.towerPlatform = this;

                for (int index = t.range; index < 3; ++index)
                {
                    if (!curConveyor.isFinalConveyorBelt)
                    {
                        curConveyor = curConveyor.nextConveyorBelt;
                    }
                }

                curConveyor.OnEnemyEnter.AddListener(t.AddEnemyToRange);

                for (int index = 1; index < t.range; ++index)
                {
                    if (!curConveyor.isFinalConveyorBelt)
                    {
                        curConveyor = curConveyor.nextConveyorBelt;
                    }
                }

                curConveyor.OnEnemyLeave.AddListener(t.RemoveEnemyFromRange);

                hasTower = true;

				GameManager.gm.RemoveScrap(GameManager.gm.basicTowerCost);
			}
		}
	}
}
