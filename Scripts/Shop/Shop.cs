using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Wallet _playerWallet;
    [SerializeField] private ShopItemData[] Assortment;
    [SerializeField] private GameObject _shopItemPrefab;

    [SerializeField] private Vector3 _startPos = new Vector3(-340f, 180f, 0f);
    [SerializeField] private int _colNumber = 3;
    [SerializeField] private float _widthOffset = 340f;
    [SerializeField] private float _heightOffset = 140f;

    private Transform _container;

    private float _distance = 1050f;
    private float _stepDistance = 210f;
    private float _time = 0.1f;

    private void Awake()
    {
        _container = transform.Find("Container").gameObject.transform;
        InitShop();
    }

    private void OnEnable()
    {
        TransitionAnimation(true);
    }

    private void InitShop()
    {
        for (int i = 0; i < Assortment.Length; i++)
        {
            var data = Assortment[i];
            var item = Instantiate(_shopItemPrefab, _container);
            item.GetComponent<ShopItemUI>().Init(data, this);

            Vector3 pos = GetGridPosition(i);
            item.transform.localPosition = pos;
        }
    }

    private Vector3 GetGridPosition(int index)
    {
        int col = index % _colNumber;
        int row = index / _colNumber;

        return new Vector3(_startPos.x + (col * _widthOffset),
            _startPos.y - (row * _heightOffset), 0);
    }

    public void TryBuy(ShopItemData item)
    {
        if (_playerWallet.Remove(item.Price))
        {
            Debug.Log("Item is Bought !");
        }
    }

    public void TransitionAnimation(bool isOpen)
    {
        StartCoroutine(TransitionAnimationCourutine(isOpen));
    }

    private IEnumerator TransitionAnimationCourutine(bool isOpen)
    {
        float currentDistance = 0;
        float step = isOpen ? _stepDistance : _stepDistance * -1f;
        while (Mathf.Abs(currentDistance) != _distance)
        {
            transform.Translate(new Vector2(0, step));
            currentDistance += step;
            yield return new WaitForSeconds(_time);
        }
        gameObject.SetActive(isOpen);
    }
}
