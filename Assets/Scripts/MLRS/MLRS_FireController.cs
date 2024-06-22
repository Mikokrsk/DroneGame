using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MLRS_FireController : MonoBehaviour
{
    public bool bolean;
    [SerializeField] private List<MLRS_RocketController> rockets;

    private void Awake()
    {
        rockets.AddRange(gameObject.GetComponentsInChildren<MLRS_RocketController>());
    }

    private void Update()
    {
        if (bolean)
        {
            bolean = false;
            rockets.First().RocketLaunch();
            rockets.Remove(rockets.First());
        }
    }

}