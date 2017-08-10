using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapButton : MonoBehaviour
{
    public float lerpSpeed;
    public GameObject target;
    public EnemyType enemyType;

    private bool doLerp;

    void Start ()
    {
        lerpSpeed = Screen.height * 1.3f;
    } 

    void Update ()
    {
        if(doLerp)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, lerpSpeed * Time.deltaTime);
        }

        if(Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            GameManager.gm.AddScrap(ScrapValues.GetEnemyDropAmount(enemyType));
            Destroy(gameObject);
        }
    } 

    public void Clicked ()
    {
        doLerp = true;
    }
}
