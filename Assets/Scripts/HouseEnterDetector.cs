using System;
using UnityEngine;

public class HouseEnterDetector : MonoBehaviour
{
    public event Action<Player> HouseEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            HouseEnter?.Invoke(player);
        }
    }    
}
