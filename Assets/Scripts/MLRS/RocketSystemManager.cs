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

    [SerializeField] private Transform _fuseCentrePosition;
    [SerializeField] private Vector3 _fuseSize;
    [SerializeField] private Color _gizmosColor;
    [SerializeField] private Collider[] _colliders;

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
        if (_rocketCount > 0 && Fuse())
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

    private bool Fuse()
    {
        var colliders = Physics.OverlapBox(_fuseCentrePosition.transform.position, _fuseSize / 2, _fuseCentrePosition.transform.rotation);
        if (colliders.Count() > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.isTrigger == false)
                {
                    return false;
                }
            }
        }
        return true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.matrix = Matrix4x4.TRS(_fuseCentrePosition.transform.position, _fuseCentrePosition.transform.rotation, _fuseSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}