using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController  : MonoBehaviour
{
    [SerializeField] private Transform basePlatform;
    [SerializeField] private Transform turret;

    [SerializeField] private float rotationSpeedY = 10f;
    [SerializeField] private float rotationSpeedX = 10f;

    [SerializeField] private float maxRotationX = 45f;
    [SerializeField] private float minRotationX = -5f;

    [SerializeField] private InputAction _moveAction;

    [SerializeField] private float _foldBatterySpeed;
    [SerializeField] private bool _isActive;
    [SerializeField] private Vector3 _endBaseRotation;
    [SerializeField] private Vector3 _endTurretRotation;

    private void Awake()
    {
        EnableTurretControl();
    }

    private void Update()
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

    private void EnableTurretControl()
    {
        _moveAction.Enable();
    }
    private void DisableTurretControl()
    {
        _moveAction.Disable();
    }

    private void HandleTurretMovement()
    {
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();

        float rotationY = inputVector.x * rotationSpeedY * Time.deltaTime;
        float rotationX = inputVector.y * rotationSpeedX * Time.deltaTime;

        if (rotationY != 0)
        {
            RotateBasePlatform(rotationY);
        }
        if (rotationX != 0)
        {
            RotateTurret(-rotationX);
        }
    }

    private void RotateBasePlatform(float rotationY)
    {
        basePlatform.Rotate(0, rotationY, 0);
    }

    private void RotateTurret(float rotationX)
    {
        float currentXRotation = turret.localEulerAngles.x;
        currentXRotation = (currentXRotation > 180) ? currentXRotation - 360 : currentXRotation;

        float newRotationX = Mathf.Clamp(currentXRotation - rotationX, minRotationX, maxRotationX);
        turret.localEulerAngles = new Vector3(newRotationX, turret.localEulerAngles.y, turret.localEulerAngles.z);
    }

    public void FoldTurret()
    {
        if (basePlatform.localRotation.eulerAngles != _endBaseRotation && turret.localRotation.eulerAngles != _endTurretRotation)
        {
            float startBaseRotationY = basePlatform.localEulerAngles.y;
            float startTurretRotationX = turret.localEulerAngles.x;

            float newBaseRotationY = Mathf.LerpAngle(startBaseRotationY, _endBaseRotation.y, _foldBatterySpeed * Time.deltaTime);
            float newTurretRotationX = Mathf.LerpAngle(startTurretRotationX, _endTurretRotation.x, _foldBatterySpeed * Time.deltaTime);

            basePlatform.localEulerAngles = new Vector3(_endBaseRotation.x, newBaseRotationY, _endBaseRotation.z);
            turret.localEulerAngles = new Vector3(newTurretRotationX, _endTurretRotation.y, _endTurretRotation.z);
        }
    }

    private void OnDestroy()
    {
        DisableTurretControl();
    }
    private void OnDisable()
    {
        DisableTurretControl();
    }
}