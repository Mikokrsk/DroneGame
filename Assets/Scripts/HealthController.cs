using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    private void Start()
    {
        health = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log($"{gameObject.name} - Death");
    }
}