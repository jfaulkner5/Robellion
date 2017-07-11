using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    int health;

    float waveTimer = 5f; //adjust time later
    float timer;

    public GameManager instance = null;

    int enemiesLeft;

    private void Awake()
    {
        //check if instance already exists
        if (instance == null)
            
            //if not, set to this
            instance = this;

        //if instance exists but isnt this: destroy it
        else if (instance != this)
            Destroy(gameObject);

        //reloading the scene wont destroy it
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= waveTimer)
        {
            /* either start wave spawning or stop wave spawning
             * not sure which we're doing here*/
        }

        //find game objects with the enemy tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesLeft = enemies.Length;
    }
}
