using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour, IHoldItem
{
    public void Use()
    {
        Debug.Log("Pick Use!");
    }
}
