using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMoveContoller : MonoBehaviour
{
    //  private Rigidbody rb;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private CameraController _followCamera;
    [SerializeField] private float _horizontalSpeed = 1;
    [SerializeField] private float _verticalSpeed = 1;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _deceleration = 2f;
    [SerializeField] private float _maxTiltAngle = 20f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _droneRotationSpeed = 1;
    [SerializeField] private Animator _droneAnimator;
    [SerializeField] private Vector3 moveDirection;
    private void Start()
    {
        //   rb = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
    }

    void Update()
    {
        HorizontalMove();
        VerticalMove();
        DroneRotation();
        //   RotateDrone();
    }

    private void HorizontalMove()
    {
        float tiltX = 0f;
        float tiltZ = 0f;
        Vector3 targetDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            targetDirection += transform.forward;
            tiltX = _maxTiltAngle;
        }
        if (Input.GetKey(KeyCode.S))
        {
            targetDirection += -transform.forward;
            tiltX = -_maxTiltAngle;
        }
        if (Input.GetKey(KeyCode.A))
        {
            targetDirection += -transform.right;
            tiltZ = _maxTiltAngle;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetDirection += transform.right;
            tiltZ = -_maxTiltAngle;
        }

        targetDirection = targetDirection.normalized;

        moveDirection = Vector3.Lerp(moveDirection, targetDirection, _acceleration * Time.deltaTime);

        if (targetDirection == Vector3.zero)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, _deceleration * Time.deltaTime);
        }

        characterController.Move(moveDirection * Time.deltaTime * _horizontalSpeed);

        Quaternion targetRotation = Quaternion.Euler(tiltX, transform.eulerAngles.y, tiltZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void VerticalMove()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            direction = Vector3.up;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction = Vector3.down;
        }

        // rb.AddForce(direction * _horizontalForce, ForceMode.Force);

        characterController.Move(direction * Time.deltaTime * _verticalSpeed);
    }

    private void DroneRotation()
    {
        var direction = 0;
        if (Input.GetKey(KeyCode.Z))
        {
            direction = -1;
            //   transform.Rotate(0, -_droneRotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.X))
        {

            direction = 1;
            //  transform.Rotate(0, _droneRotationSpeed * Time.deltaTime, 0);
        }
        //  _droneAnimator.SetFloat("DroneRotation", direction);
        // transform.localEulerAngles = new Vector3(_droneRotationSpeed * direction * Time.deltaTime, transform.localEulerAngles.y, transform.localEulerAngles.z);

        transform.Rotate(0, direction * _droneRotationSpeed * Time.deltaTime, 0);
    }

}
