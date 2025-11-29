using UnityEngine;
using Zenject;

public class HousePortal : MonoBehaviour, IReceiveHeldItem, IFreeInteractable
{
    [SerializeField] private AudioClip _receiveDishSFX;
    private OrdersManager _ordersManager;
    private GameSceneManager _gameSceneManager;
    private PortalUI _portalUI;

    [Inject]
    public void Construct(OrdersManager ordersManager, GameSceneManager gameSceneManager,
        PortalUI portalUI)
    {
        _ordersManager = ordersManager;
        _gameSceneManager = gameSceneManager;
        _portalUI = portalUI;
    }

    public void Interact()
    {
        _portalUI.Show(CancelLoadTavernScene, ConfirmLoadTavernScene);
    }

    public bool TryReceive(BaseHoldItem heldItem)
    {
        if (heldItem.TryGetComponent(out Dish dish))
        {
            SFX.Instance.PlayAudioClip(_receiveDishSFX);
            _ordersManager.AddPlayerDish(dish.Data);
            dish.Discard();
            return true;
        }
        else
            _portalUI.Show(CancelLoadTavernScene, ConfirmLoadTavernScene);
        return false;
    }

    private void ConfirmLoadTavernScene()
    {
        _gameSceneManager.LoadTavernScene();
    }

    private void CancelLoadTavernScene() { }
}
