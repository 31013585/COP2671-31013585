using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRestore : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private int restoreAmount = 1;
    [SerializeField] private float moveSpeed = 2.5f;
    private int destroyPos = -30;

    public int RestoreAmount => restoreAmount;

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
