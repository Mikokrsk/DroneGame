using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class RocketSystemManager : MonoBehaviour
{
    public bool spawnRocket;
    [SerializeField] private bool _isActive;

    [SerializeField] private InputAction _fireAction;

    [SerializeField] private List<Transform> _rocketPositions;
    [SerializeField] private List<RocketController> _rocketControllers;
    [SerializeField] private GameObject _rocketPref;
    [SerializeField] private int _rocketCount;

    private void Awake()
    {
        _fireAction.Enable();
    }

    private void Start()
    {
        _rocketControllers = new List<RocketController>();
        GetLoadedRockets();
    }

    private void GetLoadedRockets()
    {
        _rocketControllers.Clear();
        foreach (var rocketPosition in _rocketPositions)
        {
            var rocket = rocketPosition.GetComponentInChildren<Rocket>();
            var rocketController = new RocketController(rocketPosition, rocket);
            _rocketControllers.Add(rocketController);
        }
        GetLoadedRocketsCount();
    }

    private void GetLoadedRocketsCount()
    {
        _rocketCount = _rocketControllers.Count(x => x.rocket != null);
    }

    private void Update()
    {
        if (spawnRocket)
        {
            spawnRocket = false;

            ReloadRockets();
        }
        if (_isActive)
        {
            if (_fireAction.triggered)
            {
                RocketLaunch();
            }
        }

        GetLoadedRockets();
    }

    public void RocketLaunch()
    {
        if (_rocketCount > 0)
        {
            for (int i = 0; i < _rocketControllers.Count; i++)
            {
                var rocket = _rocketControllers[i].rocket;
                if (rocket != null)
                {
                    rocket.transform.SetParent(null);
                    rocket.StartExplosion();
                    break;
                }
            }
        }
    }

    public bool ReloadRockets()
    {
        var compleate = false;

        for (int i = 0; i < _rocketControllers.Count; i++)
        {
            if (_rocketControllers[i].rocket == null)
            {
                Instantiate(_rocketPref, _rocketControllers[i].rocketPosition).GetComponent<Rocket>();
                break;
            }
        }
        return compleate;
    }

    [Serializable]
    struct RocketController
    {
        public Transform rocketPosition;
        public Rocket rocket;

        public RocketController(Transform rocketPosition, Rocket rocket = null)
        {
            this.rocketPosition = rocketPosition;
            this.rocket = rocket;
        }

        public void SetRocket(Rocket rocket = null)
        {
            this.rocket = rocket;
        }
    }

    private void OnDestroy()
    {
        _fireAction.Disable();
    }
}