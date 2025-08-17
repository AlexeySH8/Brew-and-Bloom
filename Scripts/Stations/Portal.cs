using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour , IReceivesHeldItem
{
    public void Receive(GameObject heldItem)
    {
        Debug.Log($"{heldItem.name} put in Portal");
    }
}
