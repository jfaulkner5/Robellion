using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTower : Tower {

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
}
