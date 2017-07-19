using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject target; //the target object
    public float viewAngle = 40f; //angle to look down at target
    public float speedMod = 50f; //a speed modifier
    private Vector3 targetLoc; //the coord to the point where the camera looks at
	private Vector3 tempPos; //stores temp pos of cam for zoom functions 
    private GameObject camRig;
	 
    void Start ()
	{
        camRig = transform.parent.gameObject;
    }
	

	void Update ()
	{
		targetLoc = target.transform.position; //get target's coords
		transform.LookAt(targetLoc); //makes the camera look to it

        //rotates the camera rig around focused object
        if (Input.GetKey("a"))
            camRig.transform.RotateAround(target.transform.position, Vector3.up, -speedMod * Time.deltaTime);

        if (Input.GetKey("d"))
            camRig.transform.RotateAround(target.transform.position, Vector3.up, speedMod * Time.deltaTime);

        // input checks for zoom in/out of cam in reference to local pos (pos related to parent)
        if (Input.GetKey ("w"))
		{
            tempPos = transform.localPosition;
			transform.localPosition = tempPos + Vector3.forward  * speedMod * Time.deltaTime;
		}

		if (Input.GetKey ("s"))
		{
			tempPos = transform.localPosition;
			transform.localPosition = tempPos + Vector3.back * speedMod * Time.deltaTime;
		}

        if (transform.localPosition.z >= 3)
            transform.localPosition =  new Vector3(0,0,3);
        if (transform.localPosition.z <= -5)
            transform.localPosition = new Vector3(0, 0, -5);




        /*test code
		fov = Camera.main.fieldOfView;
		fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp(fov, minFov, maxFov);
		Camera.main.fieldOfView = fov;
		*/


    }

    private void LateUpdate()
    {
    	  
    }
}
