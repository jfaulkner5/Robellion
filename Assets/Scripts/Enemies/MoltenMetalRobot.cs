using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenMetalRobot : MonoBehaviour
{
    public Enemy enemy;        	//Parent enemy script.
    public MeshRenderer[] mr;   //Mesh Renderer of model so we can change its colour. 
    public Color hotColour;   
	public Color coolColour;

    public float hotTime;      	//The amount of time in seconds that the robot will be hot for.
	public float lifeDuration;	//The amount of time the robot has been alive for.
    public bool isHot;

	public float enemyDamageRange;	//The range that it will damage fellow enemies at.

    void Start ()
    {
		SetMeshRendererColour(hotColour);
    }

    void Update ()
    {
		if(isHot)
		{
			SetMeshRendererColour(Color.Lerp(mr[0].material.color, coolColour, (1.5f / hotTime) * Time.deltaTime));
			lifeDuration += Time.deltaTime;

			if(hotTime - lifeDuration < 0)
				isHot = false;
		}
    } 
		
	//Sets all the mesh renderers of the model to a specific colour.
	void SetMeshRendererColour (Color colour)
	{
		for(int x = 0; x < mr.Length; ++x)
		{
			mr[x].material.color = colour;
		}
	}

	public void DamageNearbyEnemies ()
	{
		float damage = (hotTime - lifeDuration);

		List<Enemy> enemies = GameManager.gm.enemies;

		for(int x = 0; x < enemies.Count; ++x)
		{
			Enemy e = enemies[x];

			if(Vector3.Distance(transform.position, e.transform.position) <= enemyDamageRange)
			{
				e.TakeDamage((int)damage, DamageType.Melee);
			}
		}
	}
}
