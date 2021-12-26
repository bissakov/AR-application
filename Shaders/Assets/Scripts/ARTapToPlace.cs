using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlace : MonoBehaviour
{

    [SerializeField] private GameObject[] _prefabsPBR;
    [SerializeField] private GameObject[] _prefabsFlat;
    [SerializeField] private GameObject _colorPicker;
    private int activeObject;
    private bool isPBR = true;
    private GameObject _spawnedObj;
    private ARRaycastManager _raycastManager;
    private Vector2 _touchPos;
    private static List<ARRaycastHit> _hits = new List< ARRaycastHit>();
    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }

    private bool TryGetTouchPosition(out Vector2 _touchPos)
    {
        if (Input.touchCount > 0)
        {
            if (isWithinTapArea(Input.GetTouch(0).position))
            {                
                _touchPos = Input.GetTouch(0).position;
                return true;
            }
        }

        _touchPos = default;
        return false;
    }

    private bool isWithinTapArea(Vector2 inputPos)
    {
        return inputPos.x >= 175f &&  inputPos.x <= 1785f && inputPos.y >= 90f && inputPos.y <= 990f;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 _touchPos))
        {
            return;
        }

        if (_raycastManager.Raycast(_touchPos, _hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _hits[0].pose;
            if (_spawnedObj == null)
            {
                if (isPBR) {
                    _spawnedObj = Instantiate(_prefabsPBR[activeObject], hitPose.position, _prefabsPBR[activeObject].transform.rotation);
                }
                else
                {
                    _spawnedObj = Instantiate(_prefabsFlat[activeObject], hitPose.position, _prefabsPBR[activeObject].transform.rotation);
                }
            }
            else
            {
                if (!_colorPicker.activeSelf || isPBR)
                {
                    _spawnedObj.transform.position = hitPose.position;
                }
            }
        }
    }

    public void SetPBR(bool ok)
    {
        isPBR = ok;

        if (_spawnedObj == null) return;
        else {
            Destroy(_spawnedObj);
        }
    }

    public void SetActiveObject(int i)
    {
        activeObject = i;

        if (_spawnedObj == null) return;
        else {
            Destroy(_spawnedObj);
        }
    }

}
