using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMOD_hotfixes
{
    public class Fmod_container : MonoBehaviour
    {
        [FMODUnity.EventRef] public string eventRefence;

        public FMOD.Studio.EventInstance eventInstance;

    }
}