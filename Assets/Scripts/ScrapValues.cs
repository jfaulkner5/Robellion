using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapValues : MonoBehaviour 
{
	//Tower prices.
	public static int robotArm = 150;
	public static int crusher = 500;
	public static int lazerTower = 250;
	public static int acidEtcher = 300;
	public static int drillTower = 300;

	//Tower sell.
	[Range(0.0f, 1.0f)]
	public static float towerSellPercentage = 0.75f;

    //Tower upgrade.
    public static float towerUpgradePercentage = 1.5f;

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
			case TowerType.Crusher:
				return crusher;
			case TowerType.Lazer:
				return lazerTower;
			case TowerType.AcidEtcher:
				return acidEtcher;
			case TowerType.Drill:
				return drillTower;
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
			case TowerType.Crusher:
				return (int)((float)crusher * towerSellPercentage);
			case TowerType.Lazer:
				return (int)((float)lazerTower * towerSellPercentage);
			case TowerType.AcidEtcher:
				return (int)((float)acidEtcher * towerSellPercentage);
			case TowerType.Drill:
				return (int)((float)drillTower * towerSellPercentage);
		}

		return 0;
	}

    //Returns the requested tower's sell price.
    public static int GetTowerUpgradePrice(TowerType towerType)
    {
        switch (towerType)
        {
            case TowerType.RobotArm:
                return (int)((float)robotArm * towerUpgradePercentage);
            case TowerType.Crusher:
                return (int)((float)crusher * towerUpgradePercentage);
            case TowerType.Lazer:
                return (int)((float)lazerTower * towerUpgradePercentage);
            case TowerType.AcidEtcher:
                return (int)((float)acidEtcher * towerUpgradePercentage);
            case TowerType.Drill:
                return (int)((float)drillTower * towerUpgradePercentage);
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
			case EnemyType.TowerAttraction:
				return towerAttractionRobotDrop;
			case EnemyType.EMP:
				return EMPRobotDrop;
			case EnemyType.Quick:
				return quickRobotDrop;
			case EnemyType.MoltenMetal:
				return moltenMetalRobotDrop;
		}

		return 0;
	}
}
