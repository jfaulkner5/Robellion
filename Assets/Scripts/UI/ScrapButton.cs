﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapButton : MonoBehaviour
{
    public float toContainerSpeed;
    public float toTargetSpeed;

    public Vector3 containerPos;
    public GameObject endTarget;
    public EnemyType enemyType;

    private bool moveToContainer;
    private bool moveToTarget;
    private bool begunShake;

    public float lifeTimer;

    private void Awake()
    {
        moveToContainer = true;
    }
    void Update ()
    { 
        //if(moveToContainer)
        {
            lifeTimer += Time.deltaTime;

            if (!begunShake)
            {
                moveToContainer = false;
                begunShake = true;
                StartCoroutine(ScrapShake());
            }
        }
        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, containerPos, toContainerSpeed * Time.deltaTime);

        if (moveToTarget)
            transform.position = Vector3.MoveTowards(transform.position, endTarget.transform.position, toTargetSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endTarget.transform.position) < 0.2f)
        {
            GameManager.gm.AddScrap(ScrapValues.GetEnemyDropAmount(enemyType));
            Destroy(gameObject);
        }

        //if(Vector3.Distance(transform.localPosition, containerPos) < 5.0f)
        

        if(lifeTimer >= 5.0f)
        {
            Destroy(gameObject);
        }

        transform.Rotate(0, 0, (50 + (lifeTimer * 100)) * Time.deltaTime);
    } 

    public void MoveToContainer (Vector3 containerPos)
    {
        this.containerPos = containerPos;
        moveToContainer = true;
    }

    public void Clicked ()
    {
        moveToContainer = false;
        moveToTarget = true;
        lifeTimer = 0.0f;
    }

    IEnumerator ScrapShake ()
    {
		Vector3 defaultPos = transform.position;
		Vector3 moveTo = (Random.insideUnitCircle * 10);
		moveTo += defaultPos;

        while(lifeTimer < 5.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, (50 * lifeTimer) * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveTo) < 5.0f)
            {
                moveTo = (Random.insideUnitCircle * 10);
				moveTo += defaultPos;
            }

            yield return null;
        }
    }
}
