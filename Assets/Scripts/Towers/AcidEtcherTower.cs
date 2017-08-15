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
    }

	IEnumerator AcidBurst ()
	{
		acidEffect.Play();

		yield return new WaitForSeconds(0.2f);

		acidEffect.Stop();
	}

    protected override void AttackAnimation()
    {
        StartCoroutine(AcidBurst());
    }

    protected override void AttackDamage()
    {
        for (int x = 0; x < enemiesWithinRange.Count; ++x)
        {
            if (enemiesWithinRange[x].curEffects.Count == 0)
            {
                enemiesWithinRange[x].ApplyAcidEffect(3.0f, damage, 1.0f);
            }
        }
    }
}
