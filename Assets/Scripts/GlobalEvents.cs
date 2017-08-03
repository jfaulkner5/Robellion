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

public static class GlobalEvents {
    public static UnityEventScrapPickup OnScrapPickup = new UnityEventScrapPickup();

    public static UnityEventEnemyDeathEvent OnEnemyDeath = new UnityEventEnemyDeathEvent();
    
}
