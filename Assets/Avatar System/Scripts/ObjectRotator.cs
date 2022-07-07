using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    private GameObject _rotable;
    public string rotableTag;

    void Start()
    {
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;
        SetGameObject();    
    }

    private void SetGameObject()
    {
        if (!string.IsNullOrEmpty(rotableTag))
        {
            _rotable = GameObject.FindWithTag(rotableTag);
        }
        else
        {
            _rotable = gameObject;
        }
    }

    void Update()
    {
        if (!_isRotating) return;

        if (_rotable == null) SetGameObject();
        
        // offset
        _mouseOffset = (Input.mousePosition - _mouseReference);

        // apply rotation
        _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

        // rotate
        _rotable.transform.Rotate(_rotation);

        // store mouse
        _mouseReference = Input.mousePosition;
 
    }

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;

        // store mouse
        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {

        // rotating flag
        _isRotating = false;
    }

}
