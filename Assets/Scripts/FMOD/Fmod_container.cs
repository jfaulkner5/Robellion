using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMOD_hotfixes
{
    [System.Serializable]
    public class Fmod_container //: UnityEditor.PropertyDrawer
    {

        [FMODUnity.EventRef] public string eventRefence;
        private FMOD.Studio.EventInstance _eventInstance;
        public FMOD.Studio.EventInstance EventInstance
        {
            get
            {
                _eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventRefence);
                return _eventInstance;
            }

            //shouldnt be necesarry if I do not need to change FMOD events at runtime
            //set
            //{
            //    EventInstance = value;
            //}
        }


    }


}
