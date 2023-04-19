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

    private void Start()
    {
        currentShield = maxShield;
    }

    public void TakeDamage(float damageAmount)
    {
        currentShield -= damageAmount;

        if (OnShieldChanged != null)
        {
            Debug.Log("OnShieldChanged event has subscribers.");
            OnShieldChanged.Invoke(currentShield);
            Debug.Log("OnShieldChanged event invoked with shield value: " + currentShield);
        }

        else
        {
            Debug.Log("OnShieldChanged event has no subscribers.");
        }

        Debug.Log("Shield Changed, currentHealth should be invoked.");
    }

}
