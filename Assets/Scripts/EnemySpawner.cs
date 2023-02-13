using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] GameObject[] enemy;

    [Header("Timer")]
    [SerializeField] float cooldownSpawn;
    float timeSpawn;

    [Header("Spawn Position")]
    [SerializeField] float maxPos;


    // Start is called before the first frame update
    void Start()
    {
        timeSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSpawn <= Time.time)
        {
            timeSpawn = Time.time + cooldownSpawn;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemy[0], RandomPos(), Quaternion.identity);
    }

    Vector3 RandomPos()
    {
        float randomPos = Random.Range(-maxPos, maxPos);

        return new Vector3(randomPos, 0.0f, randomPos);
    }
}
