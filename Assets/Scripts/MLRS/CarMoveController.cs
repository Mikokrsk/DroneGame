using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarMoveController : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    [SerializeField] private Wheel[] wheels;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private float maxMotorTorque = 2000;
    [SerializeField] private float maxBrakeTorque = 2000;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float maxSteeringAngle = 30;

    [SerializeField] private float currentMotorTorque = 0;
    [SerializeField] private float currentBrakeTorque = 0;
    [SerializeField] private float currentSpeed = 0;
    [SerializeField] private float currentSteeringRange = 30;

    [SerializeField] private float centreOfGravityOffset = -1f;

    [SerializeField] private KeyCode moveFWD = KeyCode.W;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveBack = KeyCode.S;
    [SerializeField] private KeyCode stopMove = KeyCode.Space;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.up * centreOfGravityOffset;

        wheels = GetComponentsInChildren<Wheel>();
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            UpdateSpeedometer();
            CheckSteeringInput();
            CheckGasPedalInput();
        }
        ApplyBrakes();
    }

    public void SetActive(bool active)
    {
        _isActive = active;
    }
    public bool GetActive()
    {
        return _isActive;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    private void UpdateSpeedometer()
    {
        float forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);
        currentSpeed = (int)(forwardSpeed * 3.6f); // Convert m/s to km/h
    }

    private void ApplyBrakes()
    {
        if (Input.GetKey(stopMove) || (currentMotorTorque == 0 && currentSpeed == 0))
        {
            currentBrakeTorque = maxBrakeTorque;
        }
        else
        {
            currentBrakeTorque = 0f;
        }

        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = currentBrakeTorque;
        }
    }

    private void CheckGasPedalInput()
    {
        float direction = 0f;

        if (Input.GetKey(moveFWD) && currentSpeed <= maxSpeed)
        {
            direction = 1;
            currentMotorTorque = maxMotorTorque;
        }
        else if (Input.GetKey(moveBack) && currentSpeed >= -maxSpeed)
        {
            direction = -1;
            currentMotorTorque = maxMotorTorque;
        }
        else
        {
            currentMotorTorque = 0f;
        }

        SetWheelTorque(direction);
    }

    private void SetWheelTorque(float direction)
    {
        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = maxMotorTorque * direction;
        }
    }

    private void CheckSteeringInput()
    {
        float steerAngle = 0f;

        if (Input.GetKey(moveRight))
        {
            steerAngle = maxSteeringAngle;
        }
        else if (Input.GetKey(moveLeft))
        {
            steerAngle = -maxSteeringAngle;
        }

        SetWheelsSteerAngle(steerAngle);
    }

    private void SetWheelsSteerAngle(float steerAngle)
    {
        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = steerAngle;
            }
        }
    }
}