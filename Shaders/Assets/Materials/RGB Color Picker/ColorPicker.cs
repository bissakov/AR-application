using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private Material[] targetMat;
    private int activeMat;
    private Material _colorPickerMat;
    private RectTransform _image;
    private Vector2 _imageSize;
    private Vector2 _imagePos; 
    private Vector2 _mousePos;
    float _saturationPosX = 1f, _saturationPosY = 1f;
    float hue = 0f;

    private void Awake()
    {
        _colorPickerMat = GetComponent<Image>().material;
        _image = GetComponent<RectTransform>();
    }
    
    private void Start()
    {
        _imageSize = _image.sizeDelta;
        _imagePos = transform.position;
        hue = Remap(_colorPickerMat.GetFloat("_HueColor"), 0, 360, 0, 1);
        // print(_imagePos);
        // print(_imageSize);
    }

    public void MovePicker()
    {
        _mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        if (isWithinBounds(_mousePos.x, _imagePos.x, _imageSize.x) && isWithinBounds(_mousePos.y, _imagePos.y, _imageSize.y)) {
            // print(_mousePos);
            float minX = _imagePos.x - _imageSize.x/4;
            float maxX = _imagePos.x + _imageSize.x/4;
            float minY = _imagePos.y - _imageSize.y/4;
            float maxY = _imagePos.y + _imageSize.y/4;

            _saturationPosX = Remap(_mousePos.x, minX, maxX, 0f, 1f);
            _saturationPosY = Remap(_mousePos.y, minY, maxY, 0f, 1f);

            _saturationPosX = Mathf.Clamp(_saturationPosX, 0f, 1f);
            _saturationPosY = Mathf.Clamp(_saturationPosY, 0f, 1f);

            _colorPickerMat.SetFloat("_SaturationX", _saturationPosX);
            _colorPickerMat.SetFloat("_SaturationY", _saturationPosY);
            hue = Remap(_colorPickerMat.GetFloat("_HueColor"), 0, 360, 0, 1);
            // print(hue);

            targetMat[activeMat].color = Color.HSVToRGB(hue, _saturationPosX, _saturationPosY);
            // print(_saturationPosX.ToString() + " " + _saturationPosY.ToString());
        }

        if (isWithinRing(_mousePos))
        {
            Vector2 dir = _mousePos - _imagePos;
            float dist = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            hue = Remap(_colorPickerMat.GetFloat("_HueColor"), 0, 360, 0, 1);
            targetMat[activeMat].color = Color.HSVToRGB(hue, _saturationPosX, _saturationPosY);

            _colorPickerMat.SetFloat("_HueColor", angle);
        }
    }

    private bool isWithinBounds(float mousePos, float imagePos, float imageSize) {
        return mousePos >= (imagePos - imageSize/4) && mousePos <= (imagePos + imageSize/4);
    }

    private bool isWithinRing(Vector2 mousePos)
    {
        // bool isInsideBiggerCicle = (Mathf.Pow(mousePos.x - _imagePos.x, 2) + Mathf.Pow(mousePos.y - _imagePos.y, 2)) < Mathf.Pow(_imageSize.x/2, 2);
        bool isInsideSmallerCircle = (Mathf.Pow(mousePos.x - _imagePos.x, 2) + Mathf.Pow(mousePos.y - _imagePos.y, 2)) < Mathf.Pow(.75f*(_imageSize.x/2), 2);
        // return isInsideBiggerCicle && !isInsideSmallerCircle;
        return !isInsideSmallerCircle;
    }

    private float Remap(float value, float OldRangeMin, float OldRangeMax, float RangeMin, float RangeMax)
    {
        return (value - OldRangeMin) / (OldRangeMax - OldRangeMin) * (RangeMax - RangeMin) + RangeMin;
    }

    public void SetActiveMat(int i)
    {
        activeMat = i;
    }
}
