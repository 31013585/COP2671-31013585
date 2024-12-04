using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int health = 2;

    [SerializeField] private float speedMin = 3f;
    [SerializeField] private float speedMax = 7f;
    private float speed;

    [SerializeField] private int rotateSpeedMin = 5;
    [SerializeField] private int rotateSpeedMax = 20;
    private float rotateSpeed;
    private float rot;

    [SerializeField] private float scaleMin = 1f;
    [SerializeField] private float scaleMax = 3f;

    [SerializeField] private int powerUpDropChance = 10;
    [SerializeField] private GameObject healthRestorePrefab;
    [SerializeField] private GameObject powerUpPrefab;

    private Rigidbody rb;

    public UnityEvent<int> OnDestroy;
    private int points;

    void Start()
    {
        speed = Random.Range(speedMin, speedMax);

        // rotate the asteroid as it moves
        rotateSpeed = Random.Range(rotateSpeedMin, rotateSpeedMax + 1);
        rotateSpeed *= Random.Range(0, 2) == 0 ? 1 : -1;

        // pick a random size for the asteroid, set its hp depending on size
        float scale = Random.Range(scaleMin, scaleMax);
        transform.localScale = Vector3.one * scale;
        if (scale >= 1.9f)
            health = Mathf.FloorToInt(health * 1.5f);
        else if (scale > 2.6f)
            health = Mathf.FloorToInt(health * 2f);

        rb = GetComponent<Rigidbody>();
    }
    
    public void SetPoints(int pts)
    {
        points = pts / 2;
    }

    void Update()
    {
        rb.velocity = Vector3.left * speed;

        if (transform.position.x > 35 || transform.position.x < -35)
            Destroy(gameObject);

        rot = rotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rot);
    }

    public void Damage()
    {
        health--;
        if (health <= 0)
        {
            OnDestroy.Invoke(points);

            SoundManager.instance.Explosion();
            ExplosionMaker.Instance.CreateExplosion(transform.position);
            Kill();
        }
        else
            SoundManager.instance.Hit();
    }

    public void Kill()
    {
        // power ups have a base chance of 10% to spawn after an asteroid getting destroyed
        // the chance increases by an addition 10% every 2 difficulty levels
        GameManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        int dropChance = powerUpDropChance + (manager.DifficultyLevel % 2) * powerUpDropChance;

        // spawn a power up
        if (Random.Range(0, 100) <= dropChance)
        {
            // randomly pick between the two powerups
            GameObject prefab = Random.Range(0, 2) == 1 ? powerUpPrefab : healthRestorePrefab;
            Instantiate(prefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
