using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PulseHandler
{
    public float timeBetweenPulses = 0.666666f;
    private float prevPulse = 0;

    public void Update()
    {
        if(prevPulse + timeBetweenPulses < Time.timeSinceLevelLoad)
        {
            PulseData pd = new PulseData();
            GlobalEvents.OnPulse.Invoke(pd);
        }
    }
}

public class GameManager : MonoBehaviour 
{
	public GameState curGameState;
    public int health;
	public int curScrap;

	public int curWave;
	public int totalWave; //total amount of waves before win check - leads to survival mode
    public float waveTime; 
    public float timer;
	public int wavesToWin;

    public static GameManager gm;
	public EnemySpawner enemySpawner;

	public List<Enemy> enemies = new List<Enemy>();
    public int enemiesLeft;

	public List<Tower> towers = new List<Tower>();
	public GameObject[] towerPrefabs;

	//Bools
	public bool canBuildOrModify;

	//Tower Costs
	public int basicTowerCost;

	//Times
	public float timeBetweenWaves;

	//UI
	public GameUI ui;

    

    //Pulse
    public PulseHandler pulse;


    void Awake ()
    {
        //check if instance already exists
		if (gm == null)
            
            //if not, set to this
			gm = this;

        //if instance exists but isnt this: destroy it
		else if (gm != this)
            Destroy(gameObject);

        //reloading the scene wont destroy it
        //DontDestroyOnLoad(gameObject);
    }

	void Start ()
	{
		curGameState = GameState.WaveDone;
		timer = timeBetweenWaves;
		canBuildOrModify = true;
		ui.isSurvival = false;
		Time.timeScale = 1;


	}

    void Update ()
    {
        pulse.Update();

		if(curGameState == GameState.WaveActive)
		{
			waveTime += Time.deltaTime;

			if(waveTime > 5.0f && enemies.Count == 0)
			{
				WaveComplete();
			}
		}
		else if(curGameState == GameState.WaveDone)
		{
			timer -= Time.deltaTime;

			if(timer <= 0)
			{
				NextWave();
			}
		}

        if(health <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

		//WIN if check 
		if(enemiesLeft == 0 && curWave == totalWave && isSurvival == false)
		{
			//confirms that it isn't between waves and survival mode isn't on
			if(curGameState == GameState.WaveDone)
			{
				return;
			}
			else
			{
                ui.WinMenu(health);	
			}
		}

		#if UNITY_EDITOR
		if (Input.GetKey ("[1]"))
		{
			Time.timeScale = 1; 
			Debug.Log("timescale 1*");
		}

		if (Input.GetKey ("[2]"))
		{
			Time.timeScale = 5;
			Debug.Log("timescale 5*");
		}	
		#endif
	}

	void NextWave ()
	{
		curWave++;
		timer = 0.0f;
        waveTime = 0.0f;
		//canBuildOrModify = false;
		curGameState = GameState.WaveActive;
		enemySpawner.SpawnEnemies(curWave);
	}

	public void WaveComplete ()
	{
		canBuildOrModify = true;
		curGameState = GameState.WaveDone;
		timer = timeBetweenWaves;
		enemies.Clear();
	}

	//Adds scrap.
	public void AddScrap (int amount)
	{
		curScrap += amount;
	}

	//Removes scrap.
	public void RemoveScrap (int amount)
	{
		curScrap -= amount;
	}
}

public enum GameState { WaveActive, WaveDone }
