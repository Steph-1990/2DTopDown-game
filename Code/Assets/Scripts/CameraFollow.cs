using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform _target;

    private Transform _transform;
    private Vector3 _offsetCamera;

    private void Start()
    {
        _transform = transform;
        _offsetCamera = _transform.position - _target.position; // On cr�e une �cart entre la position de la cam�ra et du joueur
    }

    private void Update()
    {
        _transform.position = _target.position + _offsetCamera;
    }
}
