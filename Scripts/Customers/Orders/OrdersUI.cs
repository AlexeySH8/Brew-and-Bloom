using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrdersUI : MonoBehaviour
{
    public List<OrderTemplate> OrderTemplates { get; private set; }

    private void Awake()
    {
        OrderTemplates = FindObjectsOfType<OrderTemplate>().ToList();
    }
}
