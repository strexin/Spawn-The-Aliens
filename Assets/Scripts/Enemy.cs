using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    [SerializeField] EnemyScriptableObj enemyProfile;
    [SerializeField] Player player;
    [SerializeField] Animator enemyAnimator;
    

    bool isMoving;
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log(enemyProfile.enemyMaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            navMeshAgent.destination = player.transform.position;
            enemyAnimator.SetFloat("Moving", 1.0f);
        }
        else enemyAnimator.SetFloat("Moving", 0.0f);
    }
}
