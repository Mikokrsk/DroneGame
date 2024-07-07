using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MLRSModeController : MonoBehaviour
{
    [SerializeField] private RocketSystemManager _rocketSystemManager;
    [SerializeField] private TurretMoveController _turretMoveController;
    [SerializeField] private CarMoveController _carMoveController;

    [SerializeField] private MLRSMode _currentMode;

    [SerializeField] private InputAction _switchMode;

    private void Awake()
    {
        _rocketSystemManager = GetComponentInChildren<RocketSystemManager>();
        _turretMoveController = GetComponentInChildren<TurretMoveController>();
        _carMoveController = GetComponentInChildren<CarMoveController>();

        _switchMode.Enable();
    }

    private void Update()
    {
        if (_switchMode.triggered)
        {
            SwitchMode();
        }
    }

    private void SwitchMode()
    {
        if (_currentMode == MLRSMode.Fire)
        {
            SetFireMode(false);
            SetDriveMode(true);
            _currentMode = MLRSMode.Drive;
        }
        else
        {
            if (_carMoveController.GetCurrentSpeed() < 1)
            {
                SetFireMode(true);
                SetDriveMode(false);
                _currentMode = MLRSMode.Fire;
            }
        }
    }

    private void SetFireMode(bool active)
    {
        _rocketSystemManager.SetActive(active);
        _turretMoveController.SetActive(active);
    }

    private void SetDriveMode(bool active)
    {
        _carMoveController.SetActive(active);
    }

    enum MLRSMode
    {
        Drive,
        Fire
    }
}