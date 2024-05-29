using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [SerializeField] bool isFight;
    [SerializeField] float _countBullets;
    [SerializeField] float _maxCountBullets;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _minigunBarrelBase;
    [SerializeField] Transform _minigunBarrels;
    [SerializeField] Transform _turretBase;
    [SerializeField] float _bulletForce;
    [SerializeField] Transform _bulletsObject;
    [SerializeField] GameObject _shootLight;
    [SerializeField] float _shootDelay;
    [SerializeField] float _lightDelay;
    [SerializeField] float _reloadDelay;
    [SerializeField] float _reloadDelayMax;
    [SerializeField] float _rotateBarrelsDelay;
    [SerializeField] float _rotateSpeed;
    [SerializeField] GameObject _enemy;
    private Coroutine shootingCoroutine;
    private Coroutine rotatingCoroutine;


    private void Update()
    {
        if (_reloadDelay > 0)
        {
            _reloadDelay -= Time.deltaTime;
        }

        if (isFight && _countBullets == 0 && _reloadDelay <= 0)
        {
            isFight = false;
            if (shootingCoroutine == null && rotatingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(Shooting());
                rotatingCoroutine = StartCoroutine(RotateMinigunBarrels());
            }
        }
    }

    IEnumerator Shooting()
    {
        while (_countBullets < _maxCountBullets)
        {
            _countBullets++;
            var bulletRotation = Quaternion.Euler(_minigunBarrelBase.transform.rotation.eulerAngles.x + 90,
                                                  _turretBase.transform.rotation.eulerAngles.y,
                                                  0);
            var bullet = Instantiate(_bulletPrefab, _bulletSpawnPosition.position, bulletRotation, _bulletsObject);
            bullet.GetComponent<Bullet>().enemy = _enemy;
            bullet.GetComponent<Rigidbody>().AddForce(_bulletSpawnPosition.forward * _bulletForce, ForceMode.Impulse);


            _shootLight.SetActive(true);
            yield return new WaitForSeconds(_lightDelay);
            _shootLight.SetActive(false);
            yield return new WaitForSeconds(_shootDelay);
        }

        _countBullets = 0;

        StopCoroutines();
    }

    IEnumerator RotateMinigunBarrels()
    {
        while (true)
        {
            _minigunBarrels.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime, Space.Self);
            yield return new WaitForSeconds(_rotateBarrelsDelay);
        }
    }

    private void StopCoroutines()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }

        if (rotatingCoroutine != null)
        {
            StopCoroutine(rotatingCoroutine);
            rotatingCoroutine = null;
        }

        _reloadDelay = _reloadDelayMax;
    }

    private void OnTriggerStay(Collider other)
    {
        var enemy = other.gameObject;

        if (enemy == _enemy)
        {
            AimAtTarget(_enemy.transform.position);
        }
    }

    private void AimAtTarget(Vector3 targetPosition)
    {
        Vector3 directionToEnemy = targetPosition - _turretBase.position;

        Vector3 directionToEnemyOnPlane = new Vector3(directionToEnemy.x, 0, directionToEnemy.z);
        float turretRotationY = Mathf.Atan2(directionToEnemyOnPlane.x, directionToEnemyOnPlane.z) * Mathf.Rad2Deg;

        Vector3 directionToEnemyFromBarrel = targetPosition - _minigunBarrels.position;
        float distanceOnXZPlane = new Vector3(directionToEnemyFromBarrel.x, 0, directionToEnemyFromBarrel.z).magnitude;
        float barrelRotationX = Mathf.Atan2(directionToEnemyFromBarrel.y, distanceOnXZPlane) * Mathf.Rad2Deg;

        _turretBase.rotation = Quaternion.Euler(0, turretRotationY, 0);
        _minigunBarrelBase.localRotation = Quaternion.Euler(-barrelRotationX, 0, 0);
        isFight = true;
    }

}