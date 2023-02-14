using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject enemyAttackPoint;

    [Header("Drop Collectible")]
    [SerializeField] GameObject health;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject sword;

    public void DestoyObject()
    {
        enemy.OnDead();
        DropItem();
    }

    void EnemyAttacking()
    {
        enemyAttackPoint.SetActive(true);       
    }

    void EnemyAttackingEnd()
    {
        enemyAttackPoint.SetActive(false);
        enemy.isMoving = true;
        enemy.isAttacking = false;
    }

    private void DropItem()
    {
        int dropItem = Random.Range(1, 11);
        Vector3 dropPos = new Vector3(transform.parent.position.x, 1.0f, transform.parent.position.z);
        if (dropItem == 1 || dropItem == 3 || dropItem == 5)
        {
            Instantiate(health, dropPos, Quaternion.identity);
        }
        else if (dropItem == 2 || dropItem == 4)
        {
            Instantiate(bullet, dropPos, Quaternion.identity);
        }
        else if (dropItem == 9)
        {
            Instantiate(sword, dropPos, Quaternion.identity);
        }
    }
}
