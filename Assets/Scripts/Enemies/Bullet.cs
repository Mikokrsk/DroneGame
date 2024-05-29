using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _bulletLiveTime;
    public GameObject enemy;

    private void OnEnable()
    {
        Invoke("DestroyBullet", _bulletLiveTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == enemy)
        {
            Debug.Log("Drone");
            //  _enemy.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}