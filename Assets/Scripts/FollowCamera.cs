using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float _cameraRotationSpeed = 1;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float targetRotationX;
    [SerializeField] private GameObject _droneCameraBody;
    [SerializeField] private CinemachineVirtualCamera _droneMainCamera;
    [SerializeField] private CinemachineVirtualCamera _droneSecondCamera;
    [SerializeField] private int _hightCameraPriority = 1;
    [SerializeField] private int _lowCameraPriority = 0;
    // [SerializeField] private Transform _droneTransform;
    private void Update()
    {
        CameraRotation();

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchCamera();
        }
    }

    private void LateUpdate()
    {
        //  transform.position = _droneTransform.position;
        //  transform.eulerAngles = new Vector3(transform.eulerAngles.x, _droneTransform.eulerAngles.y, 0);
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

        targetRotationX = Mathf.Clamp(_droneCameraBody.transform.localEulerAngles.x + direction * _cameraRotationSpeed, 0f, 90f);

        float newRotationX = Mathf.Lerp(transform.localEulerAngles.x, targetRotationX, _cameraRotationSpeed * Time.deltaTime);

        _droneCameraBody.transform.localEulerAngles = new Vector3(newRotationX, _droneCameraBody.transform.localEulerAngles.y, _droneCameraBody.transform.localEulerAngles.z);

    }

    private void SwitchCamera()
    {
        if (_droneMainCamera.Priority > _droneSecondCamera.Priority)
        {
            _droneSecondCamera.Priority = _hightCameraPriority;
            _droneMainCamera.Priority = _lowCameraPriority;
        }
        else
        {
            _droneMainCamera.Priority = _hightCameraPriority;
            _droneSecondCamera.Priority = _lowCameraPriority;
        }
    }

}