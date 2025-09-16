using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernPortal : MonoBehaviour, IReceiveHeldItem, IFreeInteractable
{
    public void Interact()
    {
        LoadHouseScene();
    }

    public void Receive(GameObject heldItem)
    {
        LoadHouseScene();
    }

    private void LoadHouseScene()
    {
        SceneManager.LoadScene("House");
    }
}
