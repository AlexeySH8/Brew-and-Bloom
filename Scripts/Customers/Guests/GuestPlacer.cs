using UnityEngine;

public class GuestPlacer : MonoBehaviour
{
    private void Awake()
    {
        if (GuestsManager.Instance != null &&
            GuestsManager.Instance.HasGuestForDay)
            Place();
        else
            Debug.Log("There are no guests.");
    }

    private void Place()
    {
        foreach (Guest guest in GuestsManager.Instance.GuestsForDay)
        {
            GameObject instance = Instantiate(guest.Data.GuestPrefab, 
                GetPlace(), Quaternion.identity);

            if (instance.TryGetComponent(out GuestCreature creature))
                creature.Init(guest);
            else
                Debug.LogError($"{instance.name} does not have GuestCreature");
        }
    }

    private Vector3 GetPlace()
    {
        return Vector3.one;
    }
}
