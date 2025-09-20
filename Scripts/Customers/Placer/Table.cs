using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private Transform _leftSide;
    [SerializeField] private Transform _rightSide;

    private bool _isLeftSideFree = true;
    private bool _isRightSideFree = true;

    public bool IsFree() => _isLeftSideFree || _isRightSideFree;

    public void SitDown(GameObject guest)
    {
        if (_isLeftSideFree && _isRightSideFree)
        {
            bool left = Random.value < 0.5;
            if (left) SitOnLeft(guest);
            else SitOnRight(guest);
        }
        else if (_isLeftSideFree)
            SitOnLeft(guest);
        else
            SitOnRight(guest);
    }

    private void SitOnLeft(GameObject guest)
    {
        guest.gameObject.transform.position = _leftSide.position;
        guest.GetComponent<SpriteRenderer>().flipX = false;
        _isLeftSideFree = false;
    }

    private void SitOnRight(GameObject guest)
    {
        guest.gameObject.transform.position = _rightSide.position;
        guest.GetComponent<SpriteRenderer>().flipX = true;
        _isRightSideFree = false;
    }

    public void Clear()
    {
        _isLeftSideFree = true;
        _isRightSideFree = true;
    }

    private void OnDestroy()
    {
        _isLeftSideFree = true;
        _isRightSideFree = true;
    }
}
