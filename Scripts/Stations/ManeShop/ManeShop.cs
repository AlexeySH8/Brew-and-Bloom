using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeShop : MonoBehaviour, IFreeInteractable
{
    [SerializeField] private GameObject _shop;

    private Coroutine _delivering;
    private ManeShopMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<ManeShopMovement>();
    }

    public void Interact()
    {
        if (_delivering == null)
            _shop.SetActive(true);
    }

    public void DeliverItems(List<GameObject> shoppingList) =>
        _delivering = StartCoroutine(DeliverItemsCourutine(shoppingList));

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
        _movement.EndDeliver();
        _delivering = null;
    }
}
