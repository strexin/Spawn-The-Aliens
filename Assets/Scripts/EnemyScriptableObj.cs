using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptableObj : ScriptableObject
{
    public float enemyMaxHealth;
    public float enemyAttack;
    public bool isChasingPlayer;
}
