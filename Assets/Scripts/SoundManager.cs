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

    // Use this for initialization
    void Start()
    {
        instance = this;
        sound = GetComponent<AudioSource>();
    }

    // all tower build sound
    public void OnBuild()
    {
        sound.PlayOneShot(towerBuild);
    }

    // tower attack sounds
    public void OnAttackArm()
    {
        sound.PlayOneShot(armAttack);
    }

    public void OnAttackCrusher()
    {
        sound.PlayOneShot(crusherAttack);
    }

    public void OnAttackLazer()
    {
        sound.PlayOneShot(lazerAttack);
    }

    public void OnAttackDrill()
    {
        sound.PlayOneShot(drillAttack);
    }

    public void OnAttackAcid()
    {
        sound.PlayOneShot(acidAttack);
    }

    // bot damage sounds
    public void OnBotDamage()
    {
        sound.PlayOneShot(botDamage);
    }

    // all bot death sound
    public void OnBotDeath()
    {
        sound.PlayOneShot(botDeath);
    }

    // on scrap pick up
    public void ScrapPickUp()
    {
        sound.PlayOneShot(pickUp);
    }
}
