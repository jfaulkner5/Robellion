using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource sound;

    public AudioClip acidAttack;
    public AudioClip armAttack;
    public AudioClip botDamage;
    public AudioClip botDeath;
    public AudioClip crusherAttack;
    public AudioClip drillAttack;
    public AudioClip lazerAttack;
    public AudioClip pickUp;
    public AudioClip towerBuild;

    public int numOfAudioClips = 9;

    private float[] timeOfLastPlay;
    // Use this for initialization
    void Start()
    {
        instance = this;
        sound = GetComponent<AudioSource>();

        timeOfLastPlay = new float[numOfAudioClips];
    }

    // all tower build sound
    public void OnBuild()
    {
        if ((timeOfLastPlay[8] + (towerBuild.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(towerBuild);
            timeOfLastPlay[8] = Time.timeSinceLevelLoad;
        }
    }

    // tower attack sounds
    public void OnAttackArm()
    {
        if ((timeOfLastPlay[1] + (armAttack.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(armAttack);
            timeOfLastPlay[1] = Time.timeSinceLevelLoad;
        }
    }

    public void OnAttackCrusher()
    {
        if ((timeOfLastPlay[4] + (crusherAttack.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(crusherAttack);
            timeOfLastPlay[4] = Time.timeSinceLevelLoad;
        }
    }

    public void OnAttackLazer()
    {
        if ((timeOfLastPlay[6] + (lazerAttack.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(lazerAttack);
            timeOfLastPlay[6] = Time.timeSinceLevelLoad;
        }
    }

    public void OnAttackDrill()
    {
        if ((timeOfLastPlay[5] + (drillAttack.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(drillAttack);
            timeOfLastPlay[5] = Time.timeSinceLevelLoad;
        }
    }

    public void OnAttackAcid()
    {
        if ((timeOfLastPlay[0] + (acidAttack.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(acidAttack);
            timeOfLastPlay[0] = Time.timeSinceLevelLoad;
        }
    }

    // bot damage sounds
    public void OnBotDamage()
    {
        if ((timeOfLastPlay[2] + (botDamage.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(botDamage);
            timeOfLastPlay[2] = Time.timeSinceLevelLoad;
        }
    }

    // all bot death sound
    public void OnBotDeath()
    {
        if ((timeOfLastPlay[3] + (botDeath.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(botDeath);
            timeOfLastPlay[3] = Time.timeSinceLevelLoad;
        }
    }

    // on scrap pick up
    public void ScrapPickUp()
    {
        if ((timeOfLastPlay[7] + (pickUp.length / 2)) <= Time.timeSinceLevelLoad)
        {
            sound.PlayOneShot(pickUp);
            timeOfLastPlay[7] = Time.timeSinceLevelLoad;
        }
    }
}
