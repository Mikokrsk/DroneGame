using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class CarNavigationSystem : MonoBehaviour
{
    [SerializeField] private WheeledVehicleMoveController _wheeledVehicleMoveController;
    [SerializeField] private Vector3 _targetPosition;

    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private List<Vector3> _waypoints;
    [SerializeField] private int _currentWaypointID;
    [SerializeField] private float _reachedTargetDistance;

    [SerializeField] private float angleToDir;
    [SerializeField] private float ignoreAngleToDir;
    private void Start()
    {
        var knots = _splineContainer.Spline.Knots;

        foreach (var knot in knots)
        {
            _waypoints.Add(knot.Position);
        }
        _currentWaypointID = 0;
        _targetPosition = _waypoints[_currentWaypointID];
    }

    private void Update()
    {
        if (_targetPosition == null && _wheeledVehicleMoveController.GetActive())
        {
            StopCar();
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

        if (distanceToTarget > _reachedTargetDistance)
        {
            _wheeledVehicleMoveController.MoveForward();
            Vector3 dirToMovePosition = (_targetPosition - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToMovePosition);

            if (dot > 0)
            {
                _wheeledVehicleMoveController.MoveForward();
            }
            else
            {
                _wheeledVehicleMoveController.MoveBackward();
            }

            angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

            if (angleToDir > 0 && angleToDir > ignoreAngleToDir)
            {
                _wheeledVehicleMoveController.TurnRight();
            }
            else if (angleToDir < 0 && angleToDir < ignoreAngleToDir)
            {
                _wheeledVehicleMoveController.TurnLeft();
            }
            else
            {
                _wheeledVehicleMoveController.ResetSteeringAngle();
            }
        }
        else
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        _currentWaypointID += 1;
        if (_currentWaypointID < _waypoints.Count())
        {
            _targetPosition = _waypoints[_currentWaypointID];
        }
        else
        {
            _currentWaypointID = 0;
            // StopCar();
        }
    }

    private void StopCar()
    {
        _wheeledVehicleMoveController.ApplyBrakes(1);
    }
}