using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviour : MonoBehaviour
{

    [SerializeField] private PlaneVisibilityBehaviour _pvb;

    private void Start()
    {
        _pvb._planeManager.enabled = true;
    }
}
