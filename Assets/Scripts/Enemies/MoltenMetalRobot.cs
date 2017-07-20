using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenMetalRobot : MonoBehaviour
{
    public Enemy enemy;        //Parent enemy script.
    public MeshRenderer mr;    //Mesh Renderer of model so we can change its colour. 
    public Color hotColour;   

    public float hotTime;      //The amount of time in seconds that the robot will be hot for.
    public bool isHot;

    void Start ()
    {
        mr.material.color = hotColour;
    }

    void Update ()
    {
        mr.material.color = Color.Lerp(mr.material.color, Color.white, (1.0f / hotTime) * Time.deltaTime);
    } 
}
