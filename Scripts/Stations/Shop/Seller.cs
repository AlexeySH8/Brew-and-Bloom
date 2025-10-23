using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Seller : MonoBehaviour, IFreeInteractable
{
    private Shop _shop;
    private Coroutine _delivering;
    private SellerMovement _movement;

    [Inject]
    private void Construct(Shop shop)
    {
        _shop = shop;
    }

    private void Awake()
    {
        _movement = GetComponent<SellerMovement>();
    }

    public void Interact()
    {
        if (_delivering != null || _shop.IsOpen) return;
        _shop.OpenShop();
    }

    public void DeliverItems(List<GameObject> shoppingList)
    {
        if (shoppingList.Count == 0) return;
        _delivering = StartCoroutine(DeliverItemsRoutine(shoppingList));
    }

    private IEnumerator DeliverItemsRoutine(List<GameObject> shoppingList)
    {
        yield return StartCoroutine(_movement.StartDeliver());
        yield return StartCoroutine(SpawnPurchasedItems(shoppingList));

        _movement.StartMovingAround();
        _delivering = null;
    }

    private IEnumerator SpawnPurchasedItems(List<GameObject> shoppingList)
    {
        foreach (GameObject item in shoppingList)
        {
            var prefab = Instantiate(item, transform.position, transform.rotation);
            var rb = prefab.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * 3, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
