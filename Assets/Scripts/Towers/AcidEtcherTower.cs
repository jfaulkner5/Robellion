using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEtcherTower : Tower {

    public ParticleSystem acidEffect;

    private void Awake()
    {
        if (acidEffect.isPlaying)
            acidEffect.Stop();
    }

    public override void OnPulse(PulseData pd)
    {
        base.OnPulse(pd);
        if (acidEffect.isPlaying)
            acidEffect.Stop();
    }

    protected override void AttackAnimation()
    {
        if (!acidEffect.isPlaying)
            acidEffect.Play();
    }

    protected override void AttackDamage()
    {
        for (int x = 0; x < enemiesWithinRange.Count; ++x)
        {
            if (enemiesWithinRange[x].curEffects.Count == 0)
            {
                enemiesWithinRange[x].ApplyAcidEffect(3.0f, 1, 1.0f);
            }
        }
    }
}
