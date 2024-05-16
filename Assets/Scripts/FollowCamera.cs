using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float _cameraRotationSpeed = 1;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float targetRotationX;

    [SerializeField] private Transform _droneTransform;
    private void Update()
    {
        CameraRotation();
    }

    private void LateUpdate()
    {
        transform.position = _droneTransform.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _droneTransform.eulerAngles.y, 0);
    }

    public void CameraRotation()
    {
        var direction = 0;
        if (Input.GetKey(KeyCode.C))
        {
            direction = -1;
        }
        else if (Input.GetKey(KeyCode.V))
        {
            direction = 1;
        }

        targetRotationX = Mathf.Clamp(transform.localEulerAngles.x + direction * _cameraRotationSpeed, 0f, 90f);

        float newRotationX = Mathf.Lerp(transform.localEulerAngles.x, targetRotationX, _cameraRotationSpeed * Time.deltaTime);

        transform.localEulerAngles = new Vector3(newRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);

    }

}