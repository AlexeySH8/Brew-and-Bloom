using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalUI : MonoBehaviour
{
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _confirmButton;

    private Action _onCancel;
    private Action _onConfirm;

    private void Awake()
    {
        _cancelButton.onClick.AddListener(OnCancelPressed);
        _confirmButton.onClick.AddListener(OnConfirmPressed);
    }

    public void Show(Action onCancel, Action onConfirm)
    {
        gameObject.SetActive(true);
        _onCancel = onCancel;
        _onConfirm = onConfirm;
    }

    private void OnCancelPressed()
    {
        _onCancel?.Invoke();
        Hide();
    }

    private void OnConfirmPressed()
    {
        _onConfirm?.Invoke();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
