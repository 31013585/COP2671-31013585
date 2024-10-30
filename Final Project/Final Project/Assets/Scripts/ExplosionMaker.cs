using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionMaker : MonoBehaviour
{
    public static ExplosionMaker Instance;

    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
        Instance = this;
    }
    
    public void CreateExplosion(Vector3 worldPos)
    {
        GameObject explosion = Instantiate(explosionPrefab, worldPos, explosionPrefab.transform.rotation);
    }
}
