using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int health = 5;

    [SerializeField] private float xScreenLimit = 15f;
    [SerializeField] private float yScreenLimit = 10f;

    [SerializeField] private Transform laserSpawnPoint;
    [SerializeField] private GameObject laserPrefab;

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;

    [SerializeField] private float shootDelay = 0.5f;
    private float shootTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (shootTime <= 0 && Input.GetKeyDown(KeyCode.Space))
            SpawnLaser();

        if (shootTime > 0)
            shootTime -= Time.deltaTime;
        else
            shootTime = 0;

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

    public void Damage()
    {
        health--;
        if (health <= 0)
        {
            SoundManager.instance.Explosion();
            ExplosionMaker.Instance.CreateExplosion(transform.position);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Asteroid"))
        {
            SoundManager.instance.Explosion();
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void SpawnLaser()
    {
        LaserProjectile laser = Instantiate(laserPrefab).GetComponent<LaserProjectile>();
        laser.transform.position = laserSpawnPoint.position;
        laser.Setup(true, true);

        shootTime = shootDelay;
    }
}
