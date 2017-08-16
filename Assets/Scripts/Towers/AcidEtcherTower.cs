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
        for (int index = enemiesWithinRange.Count - 1; index >= 0; --index)
        {
            if (enemiesWithinRange[index].curEffects.Count == 0)
            {
                if(enemiesWithinRange[index] != null)
                    enemiesWithinRange[index].ApplyAcidEffect(3.0f, damage, 1.0f);
            }
        }
    }
}
