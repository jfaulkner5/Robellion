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

	void Start ()
	{
		healthBar.maxValue = GameManager.gm.health;
	}

	void Update ()
	{
		curHealthText.text = GameManager.gm.health + " Lives";
		scrapText.text = GameManager.gm.curScrap.ToString();
		curWaveText.text = "Wave " + GameManager.gm.curWave;
		healthBar.value = GameManager.gm.health;

		//If the wave is active, then display the time elapsed so far into the wave.
		//If the wave is done, then display the time remaining before next wave.
		if(GameManager.gm.curGameState == GameState.WaveActive)
		{
			timeText.text = "Time Elapsed\n<size=50>" + (int)GameManager.gm.waveTime + "s</size>";
		}
		else
		{
			timeText.text = "Time Remaining\n<size=50>" + (int)GameManager.gm.timer + "s</size>";
		}
	}
}
