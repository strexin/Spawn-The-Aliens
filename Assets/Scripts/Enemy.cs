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

    float enemyCurrentHealth;

    bool isAlive;
    public bool isMoving;
    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyProfile.enemyMaxHealth;
        isMoving = true;
        isAlive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log(enemyProfile.enemyMaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && isAlive && !isAttacking)
        {
            navMeshAgent.destination = player.transform.position;
            enemyAnimator.SetFloat("Moving", 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isMoving = false;
            isAttacking = true;
            enemyAnimator.SetTrigger("Attacking");
        }
    }

    public void GetHit(float damage)
    {
        Debug.Log("Decrease Health");
        enemyCurrentHealth -= damage;

        if (enemyCurrentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        enemyAnimator.SetBool("Dead", true);        
    }
}
