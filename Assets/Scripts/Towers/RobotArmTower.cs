using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmTower : Tower {

    protected override void PlayAttackSound()
    {
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackArm();
    }
}
