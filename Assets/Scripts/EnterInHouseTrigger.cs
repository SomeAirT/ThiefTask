using System;
using UnityEngine;

public class EnterInHouseTrigger : MonoBehaviour
{
    public event Action<Player> OnHouseEnter;
    public event Action<Player> OnHouseExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            OnHouseEnter?.Invoke(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            OnHouseExit?.Invoke(player);
        }
    }
}
