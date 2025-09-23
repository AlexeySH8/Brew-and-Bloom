using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeShop : MonoBehaviour, IFreeInteractable
{
    [SerializeField] private Shop _shop;

    private Coroutine _delivering;
    private ManeMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<ManeMovement>();
    }

    public void Interact()
    {
        Debug.Log(_shop.IsOpen);
        if (_delivering != null || _shop.IsOpen) return;
        _shop.OpenShop();
    }

    public void DeliverItems(List<GameObject> shoppingList)
    {
        if (shoppingList.Count == 0) return;
        _delivering = StartCoroutine(DeliverItemsCourutine(shoppingList));
    }

    private IEnumerator DeliverItemsCourutine(List<GameObject> shoppingList)
    {
        yield return StartCoroutine(_movement.StartDeliver());
        foreach (GameObject item in shoppingList)
        {
            var prefab = Instantiate(item, transform.position, transform.rotation);
            var rb = prefab.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * 3, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.3f);
        }
        _movement.StartMovingAround();
        _delivering = null;
    }
}
