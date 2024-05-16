using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private bool _isExplosioned = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (!_isExplosioned)
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition;
            _rb.freezeRotation = true;
            Instantiate(_explosionPrefab, transform);
            Invoke("DestroyBomb", _destroyDelay);
        }
    }

    private void DestroyBomb()
    {
        Destroy(gameObject);
    }
}