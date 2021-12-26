using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneVisibilityBehaviour : MonoBehaviour
{
    [HideInInspector] public ARPlaneManager _planeManager;
    private void Awake()
    {
        _planeManager = GetComponent<ARPlaneManager>();
    }

    public void TogglePlaneDetection()
    {
        _planeManager.enabled = !_planeManager.enabled;

        if (_planeManager.enabled)
        {
            SetAllPlanesActive(true);
        }
        else
        {
            SetAllPlanesActive(false);
        }
    }

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in _planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
