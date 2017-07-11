using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {


    //public GameObject cameraTarget;
    //public Vector3 targetTempPos;
    public Vector3 cameraOffset;
    public Quaternion cameraOffsetQ;



	// Use this for initialization
	void Start () {
        //cameraOffset = cameraTarget.transform.position - transform.position;
        //Debug.Log(cameraOffset);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        //transform.position = cameraTarget.transform.position - cameraOffset;
    }
}
