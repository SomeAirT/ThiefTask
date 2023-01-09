using System;
using UnityEngine;

public class HouseExitDetector : MonoBehaviour
{    
    public event Action<Player> HouseExit;    

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            HouseExit?.Invoke(player);
        }
    }
}
