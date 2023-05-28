using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegistration : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        GameManager.Instance.RegisterCamera(cam);
    }

    private void OnDisable()
    {
        GameManager.Instance.UnregisterCamera(cam);
    }
}
