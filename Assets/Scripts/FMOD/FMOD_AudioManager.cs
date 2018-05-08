using UnityEngine;
using System.Collections;
using FMOD_hotfixes; //contains some shotcuts and fixes for working with fmod by me 

public class FMOD_AudioManager : MonoBehaviour
{
    public static FMOD_AudioManager instance;

    #region Hack Fixes
    enum AttackType
    {
        Acid,
        Crusher,
        Drill,
        Laser,
        Melee
    }
    #endregion

    #region FMOD EVENTS
    public FMOD_hotfixes.Fmod_container acidAttack, armAttack, crusherAttack, drillAttack, laserAttack;
    public FMOD_hotfixes.Fmod_container botDamage, botDeath;
    public FMOD_hotfixes.Fmod_container pickup;
    public FMOD_hotfixes.Fmod_container backgroundMusic;
    public FMOD_hotfixes.Fmod_container towerBuild;

    #endregion

    void Awake()
    {
        //Duplicate instance checkere
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        backgroundMusic.EventInstance.start();
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region Tower Functions

    public void OnBuild()
    {
        towerBuild.EventInstance.start();
    }

    public void OnAttack(string attackType)
    {
        switch (attackType)
        {
            default:
                break;

            case "Acid":
                acidAttack.EventInstance.start();
                break;
            case "Crusher":
                crusherAttack.EventInstance.start();
                break;
            case "Drill":
                drillAttack.EventInstance.start();
                break;
            case "Laser":
                laserAttack.EventInstance.start();
                break;
            case "Melee":
                armAttack.EventInstance.start();
                break;

        }
    }
    #endregion

    #region Bot Sounds

    public void OnBotDamage()
    {
        botDamage.EventInstance.start();
    }

    public void OnBotDeath()
    {
        botDeath.EventInstance.start();
    }
    #endregion

    #region miscellaneous

    public void OnPickupScrap()
    {
        pickup.EventInstance.start();
    }

    #endregion
}
