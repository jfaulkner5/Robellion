using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTower : Tower {

    public Animator animate;

    protected override void AttackDamage()
    {
        for(int index = 0; index < enemiesWithinRange.Count; ++ index)
        {
            enemiesWithinRange[index].TakeDamage(damage, damType);
        }
    }

    protected override void PlayAttackSound()
    {
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackCrusher();
    }

    IEnumerator Crush()
    {
        animate.Play("Crusher");

        yield return new WaitForSeconds(1f);

        animate.Play("Idle");
    }

    protected override void AttackAnimation()
    {
        StartCoroutine(Crush());
    }
}
