using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillTower : Tower {

    protected override void PlayAttackSound()
    {
        FMOD_AudioManager.instance.OnAttack("Drill");
        //GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().OnAttackDrill();
    }
}
