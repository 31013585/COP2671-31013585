using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float xScreenLimit = 15f;
    [SerializeField] private float yScreenLimit = 10f;

    [SerializeField] private Transform laserSpawnPoint;
    [SerializeField] private GameObject laserPrefab;

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnLaser();

        // get the player input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = Vector3.zero;
        move.x = horizontal * moveSpeed;
        move.y = vertical * moveSpeed;
        
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // keep player within bounds
        if (transform.position.x < -xScreenLimit)
            xPos = -xScreenLimit;
        else if (transform.position.x > xScreenLimit)
            xPos = xScreenLimit;

        if (transform.position.y < -yScreenLimit)
            yPos = -yScreenLimit;
        else if (transform.position.y > yScreenLimit)
            yPos = yScreenLimit;

        transform.position = new Vector3(xPos, yPos, 0);
        rb.velocity = move;        
    }

    private void SpawnLaser()
    {
        LaserProjectile laser = Instantiate(laserPrefab).GetComponent<LaserProjectile>();
        laser.transform.position = laserSpawnPoint.position;
        laser.Setup(true, true);
    }
}
