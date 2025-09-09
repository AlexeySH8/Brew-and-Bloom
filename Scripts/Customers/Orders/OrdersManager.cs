using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GuestsManager.Instance.OnGuestsPrepared += AcceptOrders;
    }

    private void OnDisable()
    {
        GuestsManager.Instance.OnGuestsPrepared -= AcceptOrders;
    }

    private void AcceptOrders(List<Guest> guests)
    {

    }
}
