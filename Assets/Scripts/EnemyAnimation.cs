using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject enemyAttackPoint;

    public void DestoyObject()
    {
        enemy.OnDead();
        //Destroy(enemy.gameObject, .8f);
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
}
