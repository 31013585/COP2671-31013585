using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserSpawnPoint;   

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;

    [SerializeField] private float laserShootTime = 1.5f;

    public UnityEvent<int> OnDeath;
    private int points;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        InvokeRepeating(nameof(SpawnLaser), 1f, laserShootTime);
    }
   
    public void SetPoints(int pts) => points = pts;

    public void Damage()
    {
        OnDeath.Invoke(points);
        SoundManager.instance.Explosion();
        ExplosionMaker.Instance.CreateExplosion(gameObject.transform.position);
        Destroy(gameObject);
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
