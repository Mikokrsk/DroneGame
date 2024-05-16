using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int force = 1;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
        }
        
        rb.AddForce(direction * force, ForceMode.Force);
    }
}
