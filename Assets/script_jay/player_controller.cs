using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    private Vector3 move;
    private player_class p_c; //refers to the player_class script to get functions

    void Start()
    {
        p_c = GetComponent<player_class>();
    }


    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // jump = Input.GetButton("Jump");
        move = (v * Vector3.forward + h * Vector3.right).normalized;

    }

    private void FixedUpdate()
    {
        // Call the Move function of the player controller
        p_c.Move(move);

    }
}