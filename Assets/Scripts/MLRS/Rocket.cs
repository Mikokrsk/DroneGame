using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _launchForce;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private Bomb bomb;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _afterTurnDelay;
    [SerializeField] private float _afterExplosionDelay;
    [SerializeField] private bool _isTurning = false;
    [SerializeField] private bool _isActive = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();
        bomb = GetComponent<Bomb>();
    }

    public void StartExplosion()
    {
        _rb.isKinematic = false;
        _rb.AddForce(transform.forward * _launchForce, ForceMode.Impulse);
        _particleSystem.Play();
        Invoke("BeginTurnAfterDelay", _afterTurnDelay);
        Invoke("BeginExplosionAfterDelay", _afterExplosionDelay);
    }

    private void BeginTurnAfterDelay()
    {
        _isTurning = true;
    }
    private void BeginExplosionAfterDelay()
    {
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        if (_isTurning)
        {
            Quaternion targetRotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, targetRotation, Time.deltaTime * _rotationSpeed));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive)
        {
            Debug.Log("RocketDestroy");
            _isActive = false;
            _isTurning = false;
            _rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            _particleSystem.Stop();
            bomb.StartExplosion();
        }
    }
}