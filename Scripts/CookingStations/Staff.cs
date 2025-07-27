using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IHoldItem
{
    public void Use()
    {
        Debug.Log("Staff Use!");
    }
}
