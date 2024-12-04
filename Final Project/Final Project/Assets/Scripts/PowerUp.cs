using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 2.5f;
    private int destroyPos = -30;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.left * moveSpeed;

        if (transform.position.x <= destroyPos)
            Destroy(gameObject);
    }
}
