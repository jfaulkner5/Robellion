﻿using System.Collections;
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
    public int starScore; //stores the final score for lvl
    public float starTotal; //total stars per level

    public Text starText; //textbox to show score

    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject textHUD;
    public bool isSurvival; // determines survive mode

    // stars 
    public GameObject StarOne;
    public GameObject StarTwo;
    public GameObject StarThree;

    void Start ()
	{
		healthBar.maxValue = GameManager.gm.health;
        isSurvival = false;

        //reset star score

        StarOne.SetActive(false);
        StarTwo.SetActive(false);
        StarThree.SetActive(false);
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

    public void CreateScrapIcon (EnemyType enemyType, Vector3 enemyPos)
    {
        Vector3 pos = new Vector3(Random.Range(-scrapIconParent.rect.width / 2, scrapIconParent.rect.width / 2), Random.Range(-scrapIconParent.rect.height / 2, scrapIconParent.rect.height / 2), 0); 
        GameObject scrap = Instantiate(scrapIconPrefab, enemyPos, Quaternion.identity, scrapIconParent.transform);

        ScrapButton scrapScript = scrap.GetComponent<ScrapButton>();
        scrapScript.enemyType = enemyType;
        scrapScript.endTarget = scrapIconTarget;
        scrapScript.MoveToContainer(pos);
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

        isSurvival = true;

        //score calculator
        starScore = (int)health / (int)starTotal;
        Debug.Log("health: " + health);
        Debug.Log("starScore: " + starScore);
        Debug.Log("total stars: " + starTotal);
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

    public void StarSystem()
    {
        if (starScore == 1)
        {
            StarOne.SetActive(true);
        }
        if (starScore == 2)
        {
            StarOne.SetActive(true);
            StarTwo.SetActive(true);
        }

        if (starScore == 3)
        {
            StarOne.SetActive(true);
            StarTwo.SetActive(true);
            StarThree.SetActive(true);
        }

    }
}
