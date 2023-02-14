using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    IObjectPool<Enemy> enemyPool;

    [Header("Object Reference")]
    [SerializeField] Enemy[] enemy;

    [Header("Timer")]
    [SerializeField] float cooldownSpawn;
    float timeSpawn;

    [Header("Spawn Position")]
    [SerializeField] float maxPos;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease, OnDestroyEnemy, maxSize:25);
    }

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
            enemyPool.Get();
        }
    }

    #region Enemy Object Pooling
    private Enemy CreateEnemy()
    {
        int enemyType = Random.Range(0, 2);
        Enemy alien = Instantiate(enemy[enemyType]);
        alien.SetPool(enemyPool);
        return alien;
    }

    private void OnGet(Enemy alien)
    {
        alien.gameObject.SetActive(true);
        alien.transform.position = RandomPos();
    }

    private void OnRelease(Enemy alien)
    {
        alien.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(Enemy alien)
    {
        Destroy(alien.gameObject);
    }
    #endregion

    Vector3 RandomPos()
    {
        float randomPos = Random.Range(-maxPos, maxPos);

        return new Vector3(randomPos, 0.0f, randomPos);
    }
}
