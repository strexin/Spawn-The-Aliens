using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    public void DestoyObject()
    {
        Destroy(enemy.gameObject);
    }

    void EnemyAttacking()
    {
        Debug.Log("Player Hit");
    }

    void EnemyAttackingEnd()
    {
        enemy.isMoving = true;
        enemy.isAttacking = false;
    }
}
