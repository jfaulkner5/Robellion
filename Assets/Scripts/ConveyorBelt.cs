using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the enemies on the belt as well as positioning for those enemies.
/// </summary>
public class ConveyorBelt : MonoBehaviour
{
    public ConveyorBeltType type;
    public AxisOrientation axisOrientation;
    public float speed = 1.0f;
    public List<Enemy> curEnemies = new List<Enemy>();
    public ConveyorBelt nextConveyorBelt;   //The conveyor belt that this one pushes objects onto.
	public bool isFinalConveyorBelt;		//Is this the final conveyor belt in the chain?

    [System.Serializable]
    public class UnityEventEnemyEvent : UnityEngine.Events.UnityEvent<EnemyData> { }
    public UnityEventEnemyEvent OnEnemyEnter;
    public UnityEventEnemyEvent OnEnemyLeave;


    //Returns the position that the enemy needs to move to on the next conveyor belt.
    //Position also has the enemy's horizontal offset based on the conveyor belt orientation.
    public Vector3 GetNextConveyorBeltPosition (float enemyHorizontalOffset)
    {
        Vector3 pos = nextConveyorBelt.transform.position;  //Set a base position for the next pos without the added offset.

        //Is the next conveyor belt moving along a different axis direction than the current one?
        if (axisOrientation != nextConveyorBelt.axisOrientation)
        {
            //Is the current conveyor belt moving along the X axis and the next one moving along the Z axis?
            if (axisOrientation == AxisOrientation.X && nextConveyorBelt.axisOrientation == AxisOrientation.Z)
            {
				pos = new Vector3(pos.x - enemyHorizontalOffset, pos.y, pos.z + enemyHorizontalOffset);
            }
            else
            {
				pos = new Vector3(pos.x - enemyHorizontalOffset, pos.y, pos.z + enemyHorizontalOffset);
            }
        }
        else
        {
            //Is the current conveyor belt moving along the X axis?
            if (axisOrientation == AxisOrientation.X)
            {
                pos = new Vector3(pos.x, pos.y, pos.z + enemyHorizontalOffset);
            }
            else
            {
                pos = new Vector3(pos.x - enemyHorizontalOffset, pos.y, pos.z);
            }
        }

        return pos;
    }

    void OnTriggerEnter (Collider col)
    {
        //If the enemy enters the conveyor belt trigger.
        if (col.gameObject.tag == "Enemy")
        {
            Enemy enemy = col.GetComponent<Enemy>();

			curEnemies.Add(enemy);
            EnemyData data = new EnemyData();
            data.enemyClassData = enemy;
            OnEnemyEnter.Invoke(data);

			enemy.curConveyorBelt = this;

            if(enemy.type == EnemyType.Quick)
            {
                speed = 1.5f;
            }
        }
    }

    void OnTriggerExit (Collider col)
    {
        //If the enemy exits the conveyor belt trigger.
        if (col.gameObject.tag == "Enemy")
        {
            Enemy enemy = col.GetComponent<Enemy>();

            EnemyData data = new EnemyData();
            data.enemyClassData = enemy;
            OnEnemyLeave.Invoke(data);
			curEnemies.Remove(enemy);

            //CHANGE TO SOME SORT OF ANIMATION
			if(isFinalConveyorBelt)
			{
				enemy.GetToEndOfPath();
			}

            if(enemy.type == EnemyType.Quick)
            {
                speed = 1.0f;
            }
        }
    }
}

public enum ConveyorBeltType { Straight, Switcher }
public enum AxisOrientation { X, Z }
