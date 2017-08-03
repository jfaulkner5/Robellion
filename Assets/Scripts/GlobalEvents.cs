using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class UnityEventScrapPickup : UnityEvent<float> { }

[System.Serializable]
public class EnemyData
{
    public Enemy enemyClassData;
    public DamageType finalBlowDamageType;
}

[System.Serializable]
public class UnityEventEnemyDeathEvent : UnityEngine.Events.UnityEvent<EnemyData> { }

[System.Serializable]
public class PulseData
{

}

[System.Serializable]
public class UnityEventPulseEvent : UnityEngine.Events.UnityEvent<PulseData> { }


public static class GlobalEvents {
    public static UnityEventScrapPickup OnScrapPickup = new UnityEventScrapPickup();

    public static UnityEventEnemyDeathEvent OnEnemyDeath = new UnityEventEnemyDeathEvent();

    public static UnityEventPulseEvent OnPulse = new UnityEventPulseEvent();
}
