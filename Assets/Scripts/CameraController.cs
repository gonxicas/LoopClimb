using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Transform _player;
    private float _upTransitionDistance;
    private float _downTransitionDistance;
    private const float Size = 7.04f;

    private void Awake()
    {
        _camera = Camera.main;
        _player = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    private void Start()
    {
        _upTransitionDistance = _camera.transform.position.y + Size;
        _downTransitionDistance = _camera.transform.position.y - Size;
    }

    private void LateUpdate()
    {
        CheckPlayerPosition();
    }

    private void CheckPlayerPosition()
    {
        if (_player.position.y >= _upTransitionDistance)
        {
            transform.Translate(Vector3.up * (2 * Size));
            UpdateTransitionDistance(true);
        }

        if (_player.position.y <= _downTransitionDistance)
        {
            transform.Translate(Vector3.up * (-2 * Size));
            UpdateTransitionDistance(false);
        }
    }

    private void UpdateTransitionDistance(bool value)
    {
        var number = value ? 1 : -1;
        _upTransitionDistance += number * 2 * Size;
        _downTransitionDistance += number * 2 * Size;
    }
}