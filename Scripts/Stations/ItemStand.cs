using UnityEngine;

public class ItemStand : MonoBehaviour, IGiveHeldItem, IReceiveHeldItem
{
    [SerializeField] private Transform _standPoint;

    private GameObject _standingItem;

    public GameObject Give()
    {
        if (_standingItem == null) return null;

        _standingItem.GetComponent<Rigidbody2D>().simulated = true;
        _standingItem.transform.SetParent(null);
        GameObject item = _standingItem;
        _standingItem = null;
        return item;
        //GameObject item = Instantiate(_standingItem);
        //Destroy(_standingItem);
        //return item;
    }

    public bool Receive(GameObject heldItem)
    {
        if (_standingItem != null) return false;

        _standingItem = heldItem;
        _standingItem.GetComponent<Rigidbody2D>().simulated = false;
        _standingItem.transform.SetParent(_standPoint);
        _standingItem.transform.localPosition = Vector3.zero;

        return true;
        //_standingItem = Instantiate(heldItem);
        //_standingItem.GetComponent<Rigidbody2D>().simulated = false;
        //_standingItem.transform.position = _standPoint.position;
        //Destroy(heldItem);
    }
}
