using System.Collections.Generic;
using UnityEngine;

public class CoockingStationVisual : MonoBehaviour
{
    public Sprite Test;
    [SerializeField] private float _yContainerPos = 1.05f;
    [SerializeField] private float _xDeltaContainerPos = 0.42f;
    [SerializeField] private GameObject _containerPrefab;
    [SerializeField] private Sprite _colorSprite;

    private List<GameObject> _containersList = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            AddIngredient(Test);
        if (Input.GetKeyDown(KeyCode.T))
            ClearIngredients();
    }

    public void AddIngredient(Sprite ingredient)
    {
        GameObject container = Instantiate(_containerPrefab, transform);
        var a = new Vector2(GetXOffsetContainerPos(), _yContainerPos);
        container.transform.localPosition = new Vector2(GetXOffsetContainerPos(), _yContainerPos);
        container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ingredient;
        _containersList.Add(container);
    }

    public void ClearIngredients()
    {
        foreach (GameObject container in _containersList)
            Destroy(container);
        _containersList.Clear();    
    }

    private float GetXOffsetContainerPos()
    {
        int index = _containersList.Count;
        if (index > 0)
        {           
            int direction = (index % 2 == 1) ? 1 : -1; // odd to the right, even to the left
            int step = (index + 1) / 2;
            return step * _xDeltaContainerPos * direction;
        }
        return 0;
    }
}
