using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlatform : MonoBehaviour 
{
	public bool hasTower;
	public Vector3 facingDirection;

    public ConveyorBelt firstConveyorInRange;
    public TowerPlatform oppositePlatform;

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
			if(GameManager.gm.curScrap >= GameManager.gm.basicTowerCost)
			{
				GameObject towerPrefab = GameManager.gm.towerPrefabs[(int)towerType];

				GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
				tower.transform.eulerAngles = new Vector3(0, GetYRotation(), 0);

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
