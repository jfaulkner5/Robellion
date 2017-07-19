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

	void Update ()
	{
		scrapText.text = "Scrap\n<size=50>" + GameManager.gm.curScrap + "</size>";
		curHealthText.text = "Health\n<size=50>" + GameManager.gm.health + "</size>";
		curWaveText.text = "Wave\n<size=50>" + GameManager.gm.curWave + "</size>";

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
