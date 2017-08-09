using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapValues : MonoBehaviour 
{
	//Tower prices.
	public static int robotArm = 150;
	public static int crusher = 200;
	public static int lazerTower = 250;
	public static int acidEtcher = 250;
	public static int drillTower = 300;

	//Tower sell.
	[Range(0.0f, 1.0f)]
	public static float towerSellPercentage = 0.75f;

	//Enemy drops.
	public static int basicRobotDrop = 50;
	public static int towerAttractionRobotDrop = 35;
	public static int EMPRobotDrop = 65;
	public static int quickRobotDrop = 75;
	public static int moltenMetalRobotDrop = 75;

	//Returns the requested tower's build price.
	public static int GetTowerBuildPrice (TowerType towerType)
	{
		switch(towerType)
		{
			case TowerType.RobotArm:
				return robotArm;
				break;
			case TowerType.Crusher:
				return crusher;
				break;
			case TowerType.Lazer:
				return lazerTower;
				break;
			case TowerType.AcidEtcher:
				return acidEtcher;
				break;
			case TowerType.Drill:
				return drillTower;
				break;
		}

		return 0;
	}

	//Returns the requested tower's sell price.
	public static int GetTowerSellPrice (TowerType towerType)
	{
		switch(towerType)
		{
			case TowerType.RobotArm:
				return (int)((float)robotArm * towerSellPercentage);
				break;
			case TowerType.Crusher:
				return (int)((float)crusher * towerSellPercentage);
				break;
			case TowerType.Lazer:
				return (int)((float)lazerTower * towerSellPercentage);
				break;
			case TowerType.AcidEtcher:
				return (int)((float)acidEtcher * towerSellPercentage);
				break;
			case TowerType.Drill:
				return (int)((float)drillTower * towerSellPercentage);
				break;
		}

		return 0;
	}

	//Returns the requested enemy's drop amount.
	public static int GetEnemyDropAmount (EnemyType enemyType)
	{
		switch(enemyType)
		{
			case EnemyType.Basic:
				return basicRobotDrop;
				break;
			case EnemyType.TowerAttraction:
				return towerAttractionRobotDrop;
				break;
			case EnemyType.EMP:
				return EMPRobotDrop;
				break;
			case EnemyType.Quick:
				return quickRobotDrop;
				break;
			case EnemyType.MoltenMetal:
				return moltenMetalRobotDrop;
				break;
		}

		return 0;
	}
}
