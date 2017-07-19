using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class UnityEventScrapPickup : UnityEvent<float> { }

public static class GlobalEvents {
    public static UnityEventScrapPickup OnScrapPickup = new UnityEventScrapPickup();
	
}
