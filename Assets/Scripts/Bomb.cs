using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private float _explosionDelay = 1f;
    // [SerializeField] private bool _isExplosioned = false;
    [SerializeField] private SphereExploder _sphereExploder;
    [SerializeField] private GameObject _VolumetricObject;
    /*    private void OnCollisionEnter(Collision collision)
        {
            if (!_isExplosioned)
            {
                Exploxion()
            }
        }*/

    private void OnEnable()
    {
        Invoke("Exploxion", _explosionDelay);
    }

    private void Exploxion()
    {
        _sphereExploder.StartExploded();
        Destroy(_rb);
        // GetComponent<MeshRenderer>().enabled = false;
        Instantiate(_VolumetricObject, transform);
        Invoke("DestroyBomb", _destroyDelay);
    }

    private void DestroyBomb()
    {
        Destroy(gameObject);
    }
}