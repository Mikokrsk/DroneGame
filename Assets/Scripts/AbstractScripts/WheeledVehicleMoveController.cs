using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheeledVehicleMoveController : VehicleMoveController
{
    public bool isActive;
    [SerializeField] protected Wheel[] wheels;

    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected float maxMotorTorque = 2000;
    [SerializeField] protected float maxBrakeTorque = 2000;
    [SerializeField] protected float maxSpeed = 20;
    [SerializeField] protected float maxSteeringAngle = 30;

    [SerializeField] protected float currentMotorTorque = 0;
    [SerializeField] protected float currentBrakeTorque = 0;
    [SerializeField] protected float currentSpeed = 0;
    [SerializeField] protected float currentSteeringAngle = 0;

    [SerializeField] protected float centreOfGravityOffset = -1f;

    //SetWheelsSteerAngle
    [SerializeField] protected float targetSteerAngle = 0f;
    [SerializeField] protected float steerDuration = 2f;

    //SetWheelTorque
    [SerializeField] protected float targetWheelTorque = 0f;
    [SerializeField] protected float setWheelTorqueDuration = 2f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.up * centreOfGravityOffset;

        wheels = GetComponentsInChildren<Wheel>();
    }

    protected virtual void Update()
    {
        if (isActive)
        {
            UpdateSpeedometer();
            SmoothSetWheelTorque();
            SmoothSteering();
            if (targetWheelTorque == 0 && currentSpeed <= 1)
            {
                ApplyBrakes(1);
            }
            else
            {
                ResetBrakes();
            }
        }
        else
        {
            ApplyBrakes(1);
        }
    }

    public virtual void SetActive(bool active)
    {
        isActive = active;
    }
    public virtual bool GetActive()
    {
        return isActive;
    }

    public virtual float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    protected virtual void UpdateSpeedometer()
    {
        float forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);
        currentSpeed = (int)(forwardSpeed * 3.6f); // Convert m/s to km/h
    }

    public virtual void ApplyBrakes(float brakeTorque)
    {
        currentBrakeTorque = maxBrakeTorque * brakeTorque;
        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = currentBrakeTorque;
        }
    }

    protected virtual void SetWheelTorque(float direction)
    {
        if (currentSpeed <= maxSpeed)
        {
            float index = Mathf.Clamp(direction, -1, 1);
            targetWheelTorque = maxMotorTorque * index;
        }
        else
        {
            targetWheelTorque = 0;
        }
    }

    protected virtual void SmoothSetWheelTorque()
    {
        currentMotorTorque = Mathf.Lerp(wheels[0].WheelCollider.motorTorque, targetWheelTorque, Time.deltaTime / setWheelTorqueDuration);
        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = currentMotorTorque;
        }
    }

    protected virtual void SetWheelsSteerAngle(float steerAngle)
    {
        float index = Mathf.Clamp(steerAngle, -1, 1);
        targetSteerAngle = maxSteeringAngle * index;
    }

    protected virtual void SmoothSteering()
    {
        currentSteeringAngle = Mathf.Lerp(wheels[0].WheelCollider.steerAngle, targetSteerAngle, Time.deltaTime / steerDuration);
        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = currentSteeringAngle;
            }
        }
    }

    public override void MoveForward()
    {
        SetWheelTorque(1f);
    }
    public override void MoveBackward()
    {
        SetWheelTorque(-1f);
    }
    public override void TurnLeft()
    {
        SetWheelsSteerAngle(-1f);
    }
    public override void TurnRight()
    {
        SetWheelsSteerAngle(1f);
    }

    public virtual void ResetSteeringAngle()
    {
        SetWheelsSteerAngle(0f);
    }

    public virtual void ResetMotorTorque()
    {
        SetWheelTorque(0f);
    }

    public virtual void ResetBrakes()
    {
        ApplyBrakes(0f);
    }
}