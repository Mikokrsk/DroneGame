using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PythonCaller _pythonCaller;
    // Start is called before the first frame update
    void Start()
    {
        _pythonCaller.RunEnsureNaming();
    }
}
