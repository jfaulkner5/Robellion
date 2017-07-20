using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public GameState curGameState;
    public int health;
	public int curScrap;

	public int curWave;
    public float waveTime; 
    public float timer;
	public int wavesToWin;

    public static GameManager gm;
	public EnemySpawner enemySpawner;

	public List<Enemy> enemies = new List<Enemy>();
    public int enemiesLeft;

	//Bools
	public bool canBuildOrModify;

	//Tower Costs
	public int basicTowerCost;

	//Times
	public float timeBetweenWaves;

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
	}

    void Update ()
    {
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
}

public enum GameState { WaveActive, WaveDone }
