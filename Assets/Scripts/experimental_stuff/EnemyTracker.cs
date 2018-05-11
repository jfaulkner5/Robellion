using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{

}

//Stores data for enemies spawned and when etc 
public class EnemySpawned
{
    public GameObject spawnedEnemy;
    //TODo can I set this to only be set once?
    public float timeSpawned;

    //Removes the need to create new variables when temp storing distance to a tower 
    private _distToTower;
    public distToTower
        {
            get
            {
                
            }
        }
}
