using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTower : Tower {

    public Animator animate;

    protected override void AttackDamage()
    {
        for(int index = enemiesWithinRange.Count-1; index >= 0; --index)
        {
            if(enemiesWithinRange[index] != null)
                enemiesWithinRange[index].TakeDamage(damage, damType);
        }
    }

    protected override void PlayAttackSound()
    {
        FMOD_AudioManager.instance.OnAttack("crusher");
        //GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackCrusher();
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
