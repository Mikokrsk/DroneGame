using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarMoveController : MonoBehaviour
{
    [SerializeField] public WheeledVehicleMoveController wheeledVehicleMoveController;
    [SerializeField] private KeyCode moveFWD = KeyCode.W;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveBack = KeyCode.S;
    [SerializeField] private KeyCode stopMove = KeyCode.Space;

    protected void Update()
    {
        if (wheeledVehicleMoveController.isActive)
        {
            CheckSteeringInput();
            CheckGasPedalInput();
            CheckBrakesInput();
        }
    }

    private void CheckGasPedalInput()
    {
        if (Input.GetKey(moveFWD))
        {
            wheeledVehicleMoveController.MoveForward();
        }
        else if (Input.GetKey(moveBack))
        {
            wheeledVehicleMoveController.MoveBackward();
        }
        else
        {
            wheeledVehicleMoveController.ResetMotorTorque();
        }
    }

    private void CheckBrakesInput()
    {
        if (Input.GetKey(stopMove))
        {
            wheeledVehicleMoveController.ApplyBrakes(1f);
        }
    }

    private void CheckSteeringInput()
    {
        if (Input.GetKey(moveRight))
        {
            wheeledVehicleMoveController.TurnRight();
        }
        else if (Input.GetKey(moveLeft))
        {
            wheeledVehicleMoveController.TurnLeft();
        }
        else
        {
            wheeledVehicleMoveController.ResetSteeringAngle();
        }
    }

    public float GetCurrentSpeed()
    {
        return wheeledVehicleMoveController.GetCurrentSpeed();
    }
}