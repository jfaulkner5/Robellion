using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillTower : Tower {

    protected override void PlayAttackSound()
    {
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackDrill();
    }
}
