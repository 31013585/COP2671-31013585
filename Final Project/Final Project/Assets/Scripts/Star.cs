using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Rigidbody rb;

    private float speed = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.Rotate(Vector3.forward, Random.Range(0, 360));

        speed = Random.Range(1f, 3f);
    }

    void Update()
    {
        if (transform.position.x < -25)
        {
            Destroy(gameObject);
        }

        rb.velocity = Vector3.left * speed;
    }
}
