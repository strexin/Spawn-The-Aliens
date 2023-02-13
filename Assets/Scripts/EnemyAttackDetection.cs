using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDetection : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject;
            player.GetComponent<Player>().GetHit(enemy.enemyDamage);
        }
    }
}
