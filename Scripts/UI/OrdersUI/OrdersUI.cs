using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrdersUI : MonoBehaviour
{
    public List<OrderTemplate> OrderTemplates { get; private set; }

    [SerializeField] private TextMeshProUGUI _ordersCountText;
    [SerializeField] private Button _ordersButton;

    [SerializeField] private float _stepDistance;
    [SerializeField] private float _distance;
    [SerializeField] private float _time;

    public void Awake()
    {
        OrderTemplates = FindObjectsOfType<OrderTemplate>().ToList();
    }

    public void TextCount(int currentCount)
    {
        _ordersCountText.text = "Orders:" + currentCount.ToString();
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
            transform.Translate(new Vector2(step, 0));
            currentDistance += step;
            yield return new WaitForSeconds(_time);
        }
        _ordersButton.gameObject.SetActive(!isOpen);
    }
}
