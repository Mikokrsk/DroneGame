using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _bulletLiveTime;
    [SerializeField] private float _bulletDamage;
    public GameObject enemy;

    private void OnEnable()
    {
        Invoke("DestroyBullet", _bulletLiveTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var healthController = collision.gameObject.GetComponentInChildren<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(_bulletDamage);
            Destroy(gameObject);
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}