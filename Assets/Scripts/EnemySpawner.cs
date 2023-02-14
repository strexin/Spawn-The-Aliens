using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    IObjectPool<Enemy> enemyPool;

    [Header("Object Reference")]
    [SerializeField] Enemy[] enemy;
    [SerializeField] GameObject alertUI;

    [Header("Timer")]
    [SerializeField] float cooldownSpawn;
    [SerializeField] float increaseSpawnTimer;
    float timeSpawn;
    float increaseTimeSpawn;

    [Header("Spawn Position")]
    [SerializeField] float maxPos;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease, OnDestroyEnemy, maxSize:25);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeSpawn = Time.time;
        increaseTimeSpawn = Time.time;
        alertUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= increaseTimeSpawn && cooldownSpawn > 4)
        {
            StartCoroutine(EnemyIncreasing());
            increaseTimeSpawn = Time.time + increaseSpawnTimer;
            cooldownSpawn -= 1.0f;
        }

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

    IEnumerator EnemyIncreasing()
    {
        alertUI.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        alertUI.SetActive(false);
    }
}
