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

    private Rigidbody rb;

    public UnityEvent<int> OnDestroy;
    private int points;

    void Start()
    {
        speed = Random.Range(speedMin, speedMax);

        rotateSpeed = Random.Range(rotateSpeedMin, rotateSpeedMax + 1);
        rotateSpeed *= Random.Range(0, 2) == 0 ? 1 : -1;

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
            Destroy(gameObject);
        }
        else
            SoundManager.instance.Hit();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
