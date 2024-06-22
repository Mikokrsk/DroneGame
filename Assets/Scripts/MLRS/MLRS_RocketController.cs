using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLRS_RocketController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _launchForce;
    [SerializeField] private float _afterLaunchForce;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private Bomb bomb;
    [SerializeField] private ParticleSystem _particleSystem;
    private bool _isTurning = false;
    private bool _isActive = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        bomb = GetComponent<Bomb>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void RocketLaunch()
    {
        _rb.isKinematic = false;
        _rb.AddForce(transform.forward * _launchForce, ForceMode.Impulse);
        _particleSystem.Play();
        StartCoroutine(BeginTurnAfterDelay(1f));
    }

    private IEnumerator BeginTurnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isTurning = true;
    }

    private void Update()
    {
        if (_isTurning && _isActive)
        {
            Quaternion targetRotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, targetRotation, Time.deltaTime * _rotationSpeed));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTurning)
        {
            Debug.Log("RocketDestroy");
            _isActive = false;
            _rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            _particleSystem.Stop();
            bomb.StartExplosion();
        }
    }
}