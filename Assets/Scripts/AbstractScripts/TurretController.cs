using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class TurretController : MonoBehaviour
{
    [SerializeField] protected bool _isActive;

    [SerializeField] protected Transform _basePlatform;
    [SerializeField] protected Transform _turret;

    [SerializeField] protected float _rotationSpeedY = 10f;
    [SerializeField] protected float _rotationSpeedX = 10f;

    [SerializeField] protected float _maxRotationX = 45f;
    [SerializeField] protected float _minRotationX = -5f;

    [SerializeField] protected float _foldTurretSpeed;

    [SerializeField] protected Vector3 _endBaseRotation;
    [SerializeField] protected Vector3 _endTurretRotation;


    protected virtual void Update()
    {
        if (!_isActive)
        {
            FoldTurret();
        }
    }

    public virtual void SetActive(bool active)
    {
        _isActive = active;
    }
    public virtual bool GetActive()
    {
        return _isActive;
    }

    protected virtual void EnableTurret()
    {
        SetActive(false);
    }

    protected virtual void DisableTurret()
    {
        SetActive(false);
        FoldTurret();
    }

    protected virtual void RotateBasePlatform(float rotationY)
    {
        _basePlatform.Rotate(0, rotationY, 0);
    }

    protected virtual void RotateTurret(float rotationX)
    {
        float currentXRotation = _turret.localEulerAngles.x;
        currentXRotation = (currentXRotation > 180) ? currentXRotation - 360 : currentXRotation;

        float newRotationX = Mathf.Clamp(currentXRotation - rotationX, _minRotationX, _maxRotationX);
        _turret.localEulerAngles = new Vector3(newRotationX, _turret.localEulerAngles.y, _turret.localEulerAngles.z);
    }

    protected virtual void FoldTurret()
    {
        if (_basePlatform.localRotation.eulerAngles != _endBaseRotation && _turret.localRotation.eulerAngles != _endTurretRotation)
        {
            float startBaseRotationY = _basePlatform.localEulerAngles.y;
            float startTurretRotationX = _turret.localEulerAngles.x;

            float newBaseRotationY = Mathf.LerpAngle(startBaseRotationY, _endBaseRotation.y, _foldTurretSpeed * Time.deltaTime);
            float newTurretRotationX = Mathf.LerpAngle(startTurretRotationX, _endTurretRotation.x, _foldTurretSpeed * Time.deltaTime);

            _basePlatform.localEulerAngles = new Vector3(_endBaseRotation.x, newBaseRotationY, _endBaseRotation.z);
            _turret.localEulerAngles = new Vector3(newTurretRotationX, _endTurretRotation.y, _endTurretRotation.z);
        }
    }

    protected virtual void OnDestroy()
    {
        DisableTurret();
    }
    protected virtual void OnDisable()
    {
        DisableTurret();
    }
}
