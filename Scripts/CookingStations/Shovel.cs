using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour, IHoldItem
{
    public void Use()
    {
        Debug.Log("Shovel  Use!");
    }
}
