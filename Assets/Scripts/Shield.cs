using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shield : MonoBehaviour
{
    public float maxShield = 100f;
    public float currentShield;
    public event Action<float> OnShieldChanged;

    public void SubscribeToShieldChanged(Action<float> method)
    {
        OnShieldChanged += method;
    }

    public void UnsubscribeFromShieldChanged(Action<float> method)
    {
        OnShieldChanged -= method;
    }

    private void Awake()
    {
        currentShield = maxShield;
    }

    public void TakeDamage(float damageAmount)
    {
        currentShield -= damageAmount;
        currentShield = Mathf.Clamp(currentShield, 0, maxShield);

        if (OnShieldChanged != null)
        {
            OnShieldChanged.Invoke(currentShield);
        }

        else
        {
            Debug.Log("OnShieldChanged event has no subscribers.");
        }

    }

}
