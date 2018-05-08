using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMOD_hotfixes
{


    [System.Serializable]
    public class Fmod_container //: UnityEditor.PropertyDrawer
    {
       
        [FMODUnity.EventRef] public string eventRefence;
        public FMOD.Studio.EventInstance eventInstance;

        public void Awake()
        {
            eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventRefence);
        }
    }


    //Unsure if this is neccesary...

    //public class FMOD_eventpack : MonoBehaviour
    //{
    //    public Fmod_container test;

    //}

}