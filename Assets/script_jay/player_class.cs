using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_class : MonoBehaviour
{

    [SerializeField] private float p_MoveStr = 10;
    //[SerializeField] private float p_JumpStr = 5;
    [SerializeField] private float p_MaxVel = 25; // stored for maxAngular Velocity of Ridgidbody

    private Rigidbody p_Rigidbody;

    // Use this for initialization
    void Start()
    {
        p_Rigidbody = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().maxAngularVelocity = p_MaxVel;
    }

    public void Move(Vector3 moveDir)
    {

        p_Rigidbody.AddForce(moveDir * p_MoveStr);
        // jump is currently broken | desireable feature
        //if (jump == true)
        //p_Rigidbody.AddForce(Vector3.up * p_JumpStr, ForceMode.Impulse);
    }

}
