using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMoveContoller : MonoBehaviour
{
    //  private Rigidbody rb;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private FollowCamera _followCamera;
    [SerializeField] private float _horizontalSpeed = 1;
    [SerializeField] private float _verticalSpeed = 1;
    [SerializeField] private float _droneRotationSpeed = 1;

    private void Start()
    {
        //   rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HorizontalMove();
        VerticalMove();
        DroneRotation();
    }

    private void HorizontalMove()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        else
        {
            if (Input.GetKey(KeyCode.S))
            {
                direction += -transform.forward;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += -transform.right;
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                direction += transform.right;
            }
        }

        if (direction.magnitude > 1f)
        {
            direction = direction.normalized;
        }

        // rb.AddForce(direction * _horizontalForce, ForceMode.Force);       
        characterController.Move(direction * Time.deltaTime * _horizontalSpeed);
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
        }
        else
        {
            if (Input.GetKey(KeyCode.X))
            {
                direction = 1;
            }
        }



        // transform.localEulerAngles = new Vector3(_droneRotationSpeed * direction * Time.deltaTime, transform.localEulerAngles.y, transform.localEulerAngles.z);

        transform.Rotate(0, direction * _droneRotationSpeed * Time.deltaTime,0);
    }

}
