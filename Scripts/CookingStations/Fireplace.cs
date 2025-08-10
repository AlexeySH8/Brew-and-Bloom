using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour, IReceivesHeldItem
{
    public void Receive(GameObject heldItem) => Cook(heldItem);

    public void Cook(GameObject ingredient)
    {
        Debug.Log("FirePlace is coocked");
    }
}
