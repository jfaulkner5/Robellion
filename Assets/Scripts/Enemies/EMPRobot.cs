using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRobot : MonoBehaviour 
{
	public Enemy enemy;		//Parent enemy script.

	public float towerDisableTime;
	private float aliveTime;
	private float EMPBlastTime;
	private bool doneEMPBlast = false;

	void Start ()
	{
		EMPBlastTime = Random.Range(1.0f, 15.0f);
	}

	void Update ()
	{
		aliveTime += Time.deltaTime;

		if(aliveTime >= EMPBlastTime && !doneEMPBlast)
		{
			doneEMPBlast = true;
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
