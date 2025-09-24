using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HousePortal : MonoBehaviour, IReceiveHeldItem, IFreeInteractable
{
    public void Interact()
    {
        LoadTavernScene();
    }

    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Dish dish))
        {
            OrdersManager.Instance.AddPlayerDish(dish.Data);
            dish.GetComponent<BaseHoldItem>().Discard();
        }
        else
        {
            LoadTavernScene();
        }
    }

    private void LoadTavernScene()
    {
        SceneManager.LoadScene("Tavern");
    }
}
