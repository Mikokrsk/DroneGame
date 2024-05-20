using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _BombObject;
    [SerializeField] private float speed;

    public void AddRigidbody()
    {
        _gameObject.SetActive(true);
        Vector3 direction = _gameObject.transform.forward;
        _gameObject.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
    }
}
