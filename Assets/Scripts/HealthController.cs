using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] Slider _healthSlider;
    [SerializeField] UnityEvent deathEvent;

    private void Start()
    {
        health = maxHealth;
        _healthSlider.value = health;
        _healthSlider.maxValue = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        _healthSlider.value = health;

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log($"{gameObject.name} - Death");
        _healthSlider.gameObject.SetActive(false);
        deathEvent.Invoke();
    }
}