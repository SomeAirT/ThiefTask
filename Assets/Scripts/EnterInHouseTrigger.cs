using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterInHouseTrigger : MonoBehaviour
{
    public event Action<Collider> OnHouseEnter;
    public event Action<Collider> OnHouseExit;

    private void OnTriggerEnter(Collider other)
    {
        OnHouseEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnHouseExit?.Invoke(other);
    }
}
