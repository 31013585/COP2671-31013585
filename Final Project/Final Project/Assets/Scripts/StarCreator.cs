using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarCreator : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;

    private float waitTime = 0.5f;

    void Start()
    {
        StartCoroutine(SpawnStars());
    }

    IEnumerator SpawnStars()
    {
        while (true)
        {
            yield return null;

            GameObject star = Instantiate(starPrefab);
            star.transform.position = new Vector3(transform.position.x, Random.Range(-10f, 10f) + transform.position.y, transform.position.z);

            waitTime = Random.Range(0.25f, 1.25f);
            yield return new WaitForSeconds(waitTime);
        }

    }
}
