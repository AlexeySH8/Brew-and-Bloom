using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersRoot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
