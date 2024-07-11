using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MLRSTurretController : TurretController
{
    [SerializeField] private RocketSystemManager _rocketSystemManager;
    [SerializeField] private InputAction _moveAction;

    private void Awake()
    {
        _moveAction.Enable();
        _rocketSystemManager.SetActive(true);
    }

    protected override void Update()
    {
        if (_isActive)
        {
            HandleTurretMovement();
        }
        else
        {
            FoldTurret();
        }
    }

    private void HandleTurretMovement()
    {
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();

        float rotationY = inputVector.x * _rotationSpeedY * Time.deltaTime;
        float rotationX = inputVector.y * _rotationSpeedX * Time.deltaTime;

        if (rotationY != 0)
        {
            RotateBasePlatform(rotationY);
        }
        if (rotationX != 0)
        {
            RotateTurret(-rotationX);
        }
    }

    protected override void DisableTurret()
    {
        SetActive(false);
        FoldTurret();
        _moveAction.Disable();
        _rocketSystemManager.SetActive(false);
    }
}