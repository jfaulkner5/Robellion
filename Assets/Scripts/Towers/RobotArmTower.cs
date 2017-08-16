using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmTower : Tower {

    public Animator animate;

    //attacks everything on its side of the belt
    protected override void AttackDamage()
    {
        float horizontalOffset = 0;
        if(transform.forward == Vector3.forward || transform.forward == -Vector3.right)
        {
            horizontalOffset = -0.2f;
        }
        else
        {
            horizontalOffset = 0.2f;
        }

        for (int index = 0; index < enemiesWithinRange.Count; ++index)
        {
            if(enemiesWithinRange[index].horizontalOffsetOnConveyorBelt == horizontalOffset)
            {
                enemiesWithinRange[index].TakeDamage(damage, damType);
            }
        }
    }

    protected override void PlayAttackSound()
    {
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackArm();
    }

    IEnumerator Hit()
    {
        animate.Play("ArmTower");

        yield return new WaitForSeconds(0.4f);

        animate.Play("Idle");
    }

    protected override void AttackAnimation()
    {
        StartCoroutine(Hit());
    }
}
