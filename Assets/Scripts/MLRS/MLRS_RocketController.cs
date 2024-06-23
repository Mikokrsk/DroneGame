using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLRS_RocketController : MonoBehaviour
{
    [SerializeField] private GameObject _rocketPref;
    public Rocket currentRocket;

    private void Awake()
    {
        if (currentRocket == null)
        {
            currentRocket = Instantiate(_rocketPref, gameObject.transform).GetComponentInChildren<Rocket>();
        }
    }

    public void RocketLaunch()
    {
        currentRocket.StartExplosion();
        currentRocket = null;
    }

}