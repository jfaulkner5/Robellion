using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour 
{
	public Text scrapText;
	public Text curWaveText;
	public Text curHealthText;
	public Text timeText;
	public Slider healthBar;

    public GameObject scrapIconPrefab;
    public RectTransform scrapIconParent;
    public GameObject scrapIconTarget;

    //Star System
    public float starScore; //stores the final score for lvl
    public float starTotal; //total stars per level

    public Text starText; //textbox to show score

    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject textHUD;
    public bool isSurvival; // determines survive mode

    void Start ()
	{
		healthBar.maxValue = GameManager.gm.health;
	}

    void Update()
    {
        curHealthText.text = GameManager.gm.health + " Lives";
        scrapText.text = GameManager.gm.curScrap.ToString();
        curWaveText.text = "Wave " + GameManager.gm.curWave;
        healthBar.value = GameManager.gm.health;

        //If the wave is active, then display the time elapsed so far into the wave.
        //If the wave is done, then display the time remaining before next wave.
        if (GameManager.gm.curGameState == GameState.WaveActive)
        {
            timeText.text = "Time Elapsed\n<size=50>" + (int)GameManager.gm.waveTime + "s</size>";
        }
        else
        {
            timeText.text = "Time Remaining\n<size=50>" + (int)GameManager.gm.timer + "s</size>";
        }

        //Pause menu key - checks if paused or unpaused
        if (Input.GetKeyDown("escape"))
        {
            PauseMenu();
        }
    }

    public void CreateScrapIcon (EnemyType enemyType)
    {
        Vector3 pos = new Vector3(Random.Range(-scrapIconParent.sizeDelta.x / 2, scrapIconParent.sizeDelta.x * 2), Random.Range(-scrapIconParent.sizeDelta.y / 2, scrapIconParent.sizeDelta.y * 2), 0);
        GameObject scrap = Instantiate(scrapIconPrefab, Vector3.zero, Quaternion.identity);
        scrap.transform.parent = scrapIconParent.transform;
        scrap.transform.localPosition = pos;

        ScrapButton scrapScript = scrap.GetComponent<ScrapButton>();
        scrapScript.enemyType = enemyType;
        scrapScript.target = scrapIconTarget;
    }

    public void PauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            //Unpauses the game
            Debug.Log("Pause menu has being toggled OFF");
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            textHUD.SetActive(true);

        }
        else
        {
            //Pauses the game
            Debug.Log("Pause menu has being toggled ON");
            Time.timeScale = 0;
            textHUD.SetActive(false);
            pauseMenu.SetActive(true);
        }
    }

    public void WinMenu(int health)
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);
        textHUD.SetActive(false);

        //score calculator
        float starScore = health / starTotal;
        Debug.Log("health: " + health);
        Debug.Log("starScore: " + starScore);
        Debug.Log("total stars: " + starTotal); ;
        starText.text = "you have scored " + starScore + " out of a possible " + starTotal + " total stars";
    }

    //Pause menu buttons
    public void QuitLevel()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ButtonSurvival()
    {
        winMenu.SetActive(false);
        Time.timeScale = 1;
        textHUD.SetActive(true);
        isSurvival = true; //informs win check that survival mode is active
    }

    public void ResumeLevel()
    {
        textHUD.SetActive(true);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
