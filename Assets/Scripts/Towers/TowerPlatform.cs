using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlatform : MonoBehaviour 
{
	public bool hasTower;
	public Vector3 facingDirection;

    public ConveyorBelt firstConveyorInRange;
    public TowerPlatform oppositePlatform;

    public bool innerCorner;

	void Start ()
	{
		facingDirection = CalculateFacingDirection();
	}

	Vector3 CalculateFacingDirection ()
	{
		RaycastHit hit;

		//Check positive X
		Ray ray = new Ray(transform.position + new Vector3(1.0f, 0.2f, 0), Vector3.up);

		if(Physics.Raycast(ray, out hit, 1.0f))
		{
			if(hit.collider != null && hit.collider.name.Contains("Belt"))
				return Vector3.right;
		}

		//Check negative X
		ray = new Ray(transform.position + new Vector3(-1.0f, 0.2f, 0), Vector3.up);

		if(Physics.Raycast(ray, out hit, 1.0f))
		{
			if(hit.collider != null && hit.collider.name.Contains("Belt"))
				return Vector3.left;
		}

		//Check positive Z
		ray = new Ray(transform.position + new Vector3(0, 0.2f, 1.0f), Vector3.up);

		if(Physics.Raycast(ray, out hit, 1.0f))
		{
			if(hit.collider != null && hit.collider.name.Contains("Belt"))
				return Vector3.forward;
		}

		//Check negative Z
		ray = new Ray(transform.position + new Vector3(0, 0.2f, -1.0f), Vector3.up);

		if(Physics.Raycast(ray, out hit, 1.0f))
		{
			if(hit.collider != null && hit.collider.name.Contains("Belt"))
				return Vector3.back;
		}

		return Vector3.zero;
	}

	public void BuildTower (TowerType towerType)
	{
		//DEBUG TESTING
		if(!hasTower)
		{
            if (towerType == TowerType.Crusher)
                if (!(oppositePlatform != null && !oppositePlatform.hasTower))
                    return;

			if (GameManager.gm.curScrap >= ScrapValues.GetTowerBuildPrice(towerType))
			{
				GameObject towerPrefab = GameManager.gm.towerPrefabs[(int)towerType];

				GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
				tower.transform.eulerAngles = new Vector3(0, GetYRotation(), 0);

                Tower t = tower.GetComponent<Tower>();
                t.setLookTargets(tower.transform.position + tower.transform.forward);

                ConveyorBelt curConveyor = firstConveyorInRange;
				GameManager.gm.towers.Add(t);
				t.towerPlatform = this;
                int thing = 3;
                if (innerCorner)
                    thing = 2;
                for (int index = t.range; index < thing; ++index)
                {
                    if (!curConveyor.isFinalConveyorBelt)
                    {
                        curConveyor = curConveyor.nextConveyorBelt;
                    }
                }

                curConveyor.OnEnemyEnter.AddListener(t.AddEnemyToRange);

                int num = 0;

                if (innerCorner && t.range > 1)
                {
                    num = -1;
                }
                else
                {
                    num = 1;
                }

                for (int index = num; index < t.range + t.range - 1; ++index)
                {
                    if (!curConveyor.isFinalConveyorBelt)
                    {
                        curConveyor = curConveyor.nextConveyorBelt;
                    }
                }

                curConveyor.OnEnemyLeave.AddListener(t.RemoveEnemyFromRange);


                //this is more for adding new towers, not good for our towers
                //if (innerCorner && t.range <= 1 )
                //{
                //    curConveyor = curConveyor.nextConveyorBelt.nextConveyorBelt;

                //    curConveyor.OnEnemyEnter.AddListener(t.AddEnemyToRange);
                //    curConveyor.OnEnemyLeave.AddListener(t.RemoveEnemyFromRange);
                //}

                

                hasTower = true;

				GameManager.gm.RemoveScrap(ScrapValues.GetTowerBuildPrice(towerType));
			}
		}
	}

	int GetYRotation ()
	{
		int rot = 0;

		if(facingDirection == Vector3.right)
			rot = 90;
		if(facingDirection == Vector3.left)
			rot = -90;
		if(facingDirection == Vector3.forward)
			rot = 0;
		if(facingDirection == Vector3.back)
			rot = 180;

		return rot;
	}
}
