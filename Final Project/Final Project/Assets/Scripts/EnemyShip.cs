using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserSpawnPoint;   

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;

    [SerializeField] private float laserShootTime = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        InvokeRepeating(nameof(SpawnLaser), 1f, laserShootTime);
    }
   
    void Update()
    {
        rb.velocity = Vector3.left * moveSpeed;

        if (transform.position.x > 35 || transform.position.x < -35)
            Destroy(gameObject);
    }

    private void SpawnLaser()
    {
        LaserProjectile laser = Instantiate(laserPrefab).GetComponent<LaserProjectile>();
        laser.transform.position = laserSpawnPoint.position;
        laser.Setup(false, false);
    }
}
