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
    [SerializeField] float _rotateBarrelsDelay;
    [SerializeField] float _rotateSpeed;

    private Coroutine shootingCoroutine;
    private Coroutine rotatingCoroutine;


    private void Update()
    {
        if (isFight && _countBullets == 0)
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
    }
}