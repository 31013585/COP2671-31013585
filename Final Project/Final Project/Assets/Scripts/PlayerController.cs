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
    private bool canShoot = true;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (canShoot && Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0)
            StartCoroutine(Shoot());

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

    IEnumerator Shoot()
    {
        canShoot = false;
        SpawnLaser();
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    public void Damage()
    {
        health--;
        gameManager.UpdateHealthUI(health);
        if (health <= 0)
            Die();
    }

    public void Heal(int hp)
    {
        health += hp;
        if (health > 5) health = 5;
        gameManager.UpdateHealthUI(health);
    }

    public IEnumerator PowerUp()
    {
        moveSpeed = 12;
        shootDelay = 0.25f;

        yield return new WaitForSeconds(5);

        moveSpeed = 8;
        shootDelay = 0.5f;
        SoundManager.instance.Hit();
        yield return null;
    }

    private void Die()
    {
        SoundManager.instance.Explosion();
        ExplosionMaker.Instance.CreateExplosion(transform.position);
        gameManager.StopGame();
        Destroy(gameObject);
    }

    // explode on contact
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Asteroid"))
        {
            SoundManager.instance.Explosion();
            ExplosionMaker.Instance.CreateExplosion(transform.position);
            Destroy(collision.gameObject);
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthRestore"))
        {
            Heal(other.GetComponent<HealthRestore>().RestoreAmount);
            Destroy(other.gameObject);
            SoundManager.instance.Explosion();
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            StopCoroutine(PowerUp());
            StartCoroutine(PowerUp());
            SoundManager.instance.Explosion();
        }
    }

    private void SpawnLaser()
    {
        LaserProjectile laser = Instantiate(laserPrefab).GetComponent<LaserProjectile>();
        laser.transform.position = laserSpawnPoint.position;
        laser.Setup(true, true);
    }
}
