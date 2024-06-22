using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected float _destroyDelay = 1f;
    [SerializeField] protected float _explosionDelay = 1f;
    [SerializeField] protected float _explosionDamage;
    [SerializeField] protected SphereExploder _sphereExploder;
    [SerializeField] protected GameObject _VolumetricObject;
    [SerializeField] protected Animator _animator;
    public bool isActive;

    private void Awake()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
    }

    public virtual void StartExplosion()
    {
        if (isActive)
        {
            _explosionDelay = Random.Range(0.1f, _explosionDelay);
        }
        else
        {
            isActive = true;
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (_explosionDelay > 0)
        {
            _explosionDelay -= Time.deltaTime;
        }
        else
        {
            isActive = false;
            Explosion();
        }
    }

    protected virtual void Explosion()
    {
        _sphereExploder.StartExploded(_explosionDamage);
        Destroy(_rb);
        Instantiate(_VolumetricObject, transform);
        Invoke("DestroyBomb", _destroyDelay);
    }

    protected virtual void DestroyBomb()
    {
        Destroy(gameObject.gameObject);
    }
}