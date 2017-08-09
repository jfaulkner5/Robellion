using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTower : Tower {

    public int maxAttackNumber;

    public LineRenderer lr;
    private List<LineRenderer> lrs = new List<LineRenderer>();

    public override void OnPulse(PulseData pd)
    {
        lr.enabled = false;
        for (int index = 0; index < lrs.Count; ++index)
        {
            Destroy(lrs[index].gameObject);
        }
        lrs.Clear();
    }

    private void Awake()
    {
        lr.SetPosition(0, transform.position + new Vector3(0, 0.5f, 0));
    }

    protected override void AttackDamage()
    {
        List<Enemy> targets = new List<Enemy>();

        if(enemiesWithinRange.Count > maxAttackNumber)
        {
            while(targets.Count < maxAttackNumber)
            {
                Enemy temp = enemiesWithinRange[Random.Range(0, enemiesWithinRange.Count)];
                if (!targets.Contains(temp))
                {
                    targets.Add(temp);
                }
            }
        }
        else
        {
            targets = enemiesWithinRange;
        }

        for (int index = 0; index < targets.Count; ++index)
        {
            //line stuff here?
            lrs.Add(Instantiate(lr));
            lrs[index].enabled = true;
            lrs[index].SetPosition(1, targets[index].transform.position + new Vector3(0, 0.1f, 0));

            targets[index].TakeDamage(damage, damType);
        }
    }

    protected override void PlayAttackSound()
    {
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackLazer();
    }
}
