using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour, ICookingStation
{
    public void Cook(GameObject ingredient)
    {
        Debug.Log("FirePlace is coocked");
    }
}
