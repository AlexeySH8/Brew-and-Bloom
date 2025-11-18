using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class BaseItemHolder : MonoBehaviour, IGiveHeldItem, IReceiveHeldItem
{
    public abstract Transform ParentPoint { get; }
    public abstract int SortingOrderOffset { get; }
    public string HolderId => _holderId;
    public BaseHoldItem HeldItem => _heldItem;
    protected BaseHoldItem _heldItem;
    [SerializeField] protected string _holderId = "";

    protected virtual void Awake()
    {
        CheckChildren();
    }

    public virtual bool TryReceive(BaseHoldItem heldItem) => TryReceiveBase(heldItem);

    public bool TryReceiveBase(BaseHoldItem heldItem)
    {
        if (_heldItem != null) return false;

        if (IsCorrectItemToReceive(heldItem))
        {
            _heldItem = heldItem;
            _heldItem.SetHolder(this);
            return true;
        }
        return false;
    }

    public virtual BaseHoldItem GiveItem()
    {
        if (_heldItem == null) return null;

        BaseHoldItem item = _heldItem;
        _heldItem.SetHolder(null);
        _heldItem = null;
        return item;
    }

    public bool HasItem() => _heldItem != null;

    protected virtual bool IsCorrectItemToReceive(BaseHoldItem heldItem) => true;

    public virtual void OnItemRemoved(BaseHoldItem holdItem)
    {
        if (_heldItem == null || _heldItem != holdItem) return;
        _heldItem = null;
    }

    private void CheckChildren()
    {
        if (ParentPoint.childCount == 0) return;

        for (int i = 0; i < ParentPoint.childCount; i++)
        {
            Transform child = ParentPoint.GetChild(i);

            // If the object was manually inserted into the parent
            if (child.TryGetComponent(out BaseHoldItem holdItem) &&
                !holdItem.HasParent())
            {
                if (!TryReceive(holdItem))
                    Debug.LogError($"Object {holdItem.name} cannot be inserted into parent {this.name}");
            }
        }
    }

    protected virtual void LoadHeldItem(GameData gameData)
    {
        ItemHolderSaveData holderData = gameData.ItemHoldersSaveData
            .FirstOrDefault(h => h.HolderId == _holderId);

        if (holderData == null || string.IsNullOrEmpty(holderData.PrefabPath)) return;

        GameObject prefab = Resources.Load<GameObject>(holderData.PrefabPath);
        var item = Instantiate(prefab);
        BaseHoldItem holdItem = item.GetComponent<BaseHoldItem>();
        TryReceiveBase(holdItem);
    }

    protected virtual void SaveHeldItem(GameData gameData)
    {
        if (string.IsNullOrEmpty(_holderId))
        {
            Debug.LogError($"The ItemHolder {name} does not have ID. Item {_heldItem.name} not saved");
            return;
        }

        ItemHolderSaveData holderData = gameData.ItemHoldersSaveData
            .FirstOrDefault(h => h.HolderId == _holderId);

        if (holderData == null)
        {
            holderData = new ItemHolderSaveData() { HolderId = _holderId };
            gameData.ItemHoldersSaveData.Add(holderData);
        }
        holderData.PrefabPath = _heldItem != null ? _heldItem.PrefabPath : "";
    }

    [ContextMenu("Generate Unique Id")]
    private void GenerateUniqueId()
    {
        _holderId = GUID.Generate().ToString();
        EditorUtility.SetDirty(this);
    }
}
