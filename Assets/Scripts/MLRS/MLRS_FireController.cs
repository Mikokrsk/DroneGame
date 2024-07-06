using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class MLRS_FireController : MonoBehaviour
{
    public bool isFire;
    public bool spawnRocket;

    [SerializeField] private List<Transform> _rocketPositions;
    [SerializeField] private List<RocketController> _rocketControllers;
    [SerializeField] private GameObject _rocketPref;
    [SerializeField] private int _rocketCount;

    private void Start()
    {
        _rocketControllers = new List<RocketController>();
        UpdateAmmo();
        UpdateCountAmmo();
    }

    private void UpdateAmmo()
    {
        _rocketControllers.Clear();
        foreach (var rocketPosition in _rocketPositions)
        {
            var rocket = rocketPosition.GetComponentInChildren<Rocket>();
            var rocketController = new RocketController(rocketPosition, rocket);
            _rocketControllers.Add(rocketController);
        }
    }

    private void UpdateCountAmmo()
    {
        _rocketCount = _rocketControllers.Count(x => x.rocket != null);
    }

    private void Update()
    {
        if (isFire)
        {
            isFire = false;

            RocketLaunch();
        }

        if (spawnRocket)
        {
            spawnRocket = false;

            SpawnRocket();
        }
        UpdateCountAmmo();
    }

    public void RocketLaunch()
    {
        for (int i = 0; i < _rocketControllers.Count; i++)
        {
            var rocket = _rocketControllers[i].rocket;
            if (rocket != null)
            {
                rocket.transform.SetParent(null);
                rocket.StartExplosion();
                UpdateAmmo();
                break;
            }
        }
    }

    public bool SpawnRocket()
    {
        var compleate = false;

        for (int i = 0; i < _rocketControllers.Count; i++)
        {
            if (_rocketControllers[i].rocket == null)
            {
                Instantiate(_rocketPref, _rocketControllers[i].rocketPosition).GetComponent<Rocket>();
                UpdateAmmo();
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
}