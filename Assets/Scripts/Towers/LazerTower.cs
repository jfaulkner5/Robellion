using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTower : Tower {

    public int maxAttackNumber;

    public LineRenderer lr;

    private void Awake()
    {
        lr.SetPosition(0, transform.position + new Vector3(0, 0.5f, 0));
        lr.enabled = false;
    }

    public override void OnAlternatePulse(PulseData pd)
    {
        base.OnAlternatePulse(pd);
    }


    protected override void AttackDamage()
    {
        List<Enemy> targets = new List<Enemy>();
        if (target.type == EnemyType.TowerAttraction)
        {
            targets.Add(target);
        }
        else
        {
            if (enemiesWithinRange.Count > maxAttackNumber)
            {
                while (targets.Count < maxAttackNumber)
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
        }


        StartCoroutine(Attack(maxAttackNumber, targets));
       
    }

    IEnumerator Attack(int numberOfAttacks, List<Enemy> targets)
    {
        while (targets.Count > 0 && numberOfAttacks > 0)
        {
            for (int index = 0; index < targets.Count; ++index)
            {
                if (numberOfAttacks > 0 && targets[index] != null)
                {
                    PlayAttackSound();
                    LineRenderer lrs = Instantiate(lr);
                    lrs.enabled = true;
                    lrs.SetPosition(1, targets[index].transform.position + new Vector3(0, 0.1f, 0));
                    Destroy(lrs.gameObject, 0.1f);

                    targets[index].TakeDamage(damage, damType);

                    numberOfAttacks--;
                    yield return new WaitForSeconds(0.25f);
                }
                else
                {
                    targets.RemoveAt(index);
                }
            }
        }
    }

    public override void Upgrade()
    {
        if (GameManager.gm.curScrap >= ScrapValues.GetTowerUpgradePrice(type) && remainingNumOfUpgrades > 0)
        {
            maxAttackNumber++;
            base.Upgrade();
        }
    }

    protected override void PlayAttackSound()
    {
        FMOD_AudioManager.instance.OnAttack("laser");
        //GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackLazer();
    }
}
