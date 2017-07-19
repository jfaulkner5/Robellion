using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

    public GameObject target;//the target object
    public float viewAngle = 40; //angle to look down at target
    public float speedMod = 10.0f;//a speed modifier
    private Vector3 targetLoc;//the coord to the point where the camera looks at


    // Use this for initialization
    void Start () {
        targetLoc = target.transform.position;  //get target's coords
        transform.LookAt(targetLoc);    //makes the camera look to it

    }
	
	// Update is called once per frame
	void Update () {

        //transform.RotateAround(targetLoc, new Vector3(0.0f, 1.0f, 0.0f), viewAngle * Time.deltaTime * speedMod);
        if (Input.GetKey("a"))
        transform.RotateAround(target.transform.position, Vector3.up, -speedMod * Time.deltaTime);

        if (Input.GetKey("d"))
            transform.RotateAround(target.transform.position, Vector3.up, speedMod * Time.deltaTime);



    }

    private void LateUpdate()
    {
        //transform.position = cameraTarget.transform.position - cameraOffset;
    }
}
