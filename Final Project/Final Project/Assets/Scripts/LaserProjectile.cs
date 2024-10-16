using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    private Rigidbody rb;

    private bool isPlayerLaser;
    private Vector3 moveDir = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Setup(bool moveRight, bool isPlayer)
    {
        moveDir = new Vector3(moveSpeed * (moveRight ? 1 : -1), 0, 0);
        isPlayerLaser = isPlayer;
    }

    void Update()
    {
        rb.velocity = moveDir;

        if (transform.position.x > 25 || transform.position.x < -25)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // add asteroid and enemy/player collision
    }
}
