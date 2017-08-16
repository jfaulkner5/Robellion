using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRobot : MonoBehaviour 
{
	public Enemy enemy;		//Parent enemy script.

	public float towerDisableTime;
	private int aliveTime;
	private float EMPBlastTime;
	private bool doneEMPBlast = false;

    public ParticleSystem empBlast;

    private void Awake()
    {
        empBlast.Stop();
        GlobalEvents.OnPulse.AddListener(OnPulse);
    }

    void Start ()
	{
		EMPBlastTime = Random.Range(5, 16);
	}

	//void Update ()
	//{
	//	//aliveTime += Time.deltaTime;

	//	if(aliveTime >= EMPBlastTime && !doneEMPBlast)
	//	{
	//		doneEMPBlast = true;
	//		EMPBlast();
	//	}
	//}

    public void OnPulse(PulseData pd)
    {
        aliveTime++;

        if (aliveTime >= EMPBlastTime && !doneEMPBlast)
        {
            doneEMPBlast = true;
            EMPBlast();
        }
    }

	void EMPBlast ()
	{
        StartCoroutine(EmpBlastParticleEffect());
		for(int x = 0; x < GameManager.gm.towers.Count; ++x)
		{
            if(GameManager.gm.towers[x].type != TowerType.Crusher && GameManager.gm.towers[x].type != TowerType.RobotArm)
			    GameManager.gm.towers[x].EMPDisable(towerDisableTime);
		}
	}

    IEnumerator EmpBlastParticleEffect()
    {
        empBlast.Play();
        yield return new WaitForSeconds(empBlast.main.duration);
        empBlast.Stop();
    }
}
