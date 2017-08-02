using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRobot : MonoBehaviour 
{
	public Enemy enemy;		//Parent enemy script.

	public float towerDisableTime;
	public float aliveTime;
	public float EMPBlastTime;

	void Start ()
	{
		EMPBlastTime = Random.Range(0.0f, 6.0f);
	}

	void Update ()
	{
		aliveTime += Time.deltaTime;

		if(EMPBlastTime >= aliveTime)
		{
			EMPBlast();
		}
	}

	void EMPBlast ()
	{
		for(int x = 0; x < GameManager.gm.towers.Count; ++x)
		{
			GameManager.gm.towers[x].EMPDisable(towerDisableTime);
		}
	}
}
