using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int health;

    public float waveTimer = 5f; //adjust time later
    public float timer;

    public GameManager gameManager;

    public int enemiesLeft;

    void Awake ()
    {
        //check if instance already exists
		if (gameManager == null)
            
            //if not, set to this
			gameManager = this;

        //if instance exists but isnt this: destroy it
		else if (gameManager != this)
            Destroy(gameObject);

        //reloading the scene wont destroy it
        DontDestroyOnLoad(gameObject);
    }

    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= waveTimer)
        {
            /* either start wave spawning or stop wave spawning
             * not sure which we're doing here*/
        }
    }
}
