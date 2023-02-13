using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;

    float spawnPosX;
    Vector3 spawnEnemyPos;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosX = Random.Range(-20, 20);
        spawnEnemyPos = new Vector3(spawnPosX, 0.0f, 20.0f);
        Instantiate(enemy[0], spawnEnemyPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
