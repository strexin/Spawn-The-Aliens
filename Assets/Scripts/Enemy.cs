using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    GameObject player;
    Rigidbody rb;

    [Header("Enemy Attributes")]
    [SerializeField] EnemyScriptableObj enemyProfile;
    [SerializeField] Animator enemyAnimator;
    float enemyCurrentHealth;
    public float enemyDamage;
    bool activeEnemy;
    Vector3 randomDestination;

    [Header("Status")]
    public bool isMoving;
    public bool isAttacking;
    bool isAlive;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        activeEnemy = enemyProfile.isChasingPlayer;
        enemyCurrentHealth = enemyProfile.enemyMaxHealth;
        randomDestination = new Vector3(Random.Range(-20, 20), 0.0f, Random.Range(-20, 20));
        isMoving = true;
        isAlive = true;       
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && isAlive && !isAttacking)
        {
            if (activeEnemy)
            {
                navMeshAgent.destination = player.transform.position;
                enemyAnimator.SetFloat("Moving", 1.0f);
            } else if (!activeEnemy)
            {
                navMeshAgent.destination = randomDestination;
                enemyAnimator.SetFloat("Moving", 1.0f);
            }
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
        rb.velocity = Vector3.zero;
        GetComponent<BoxCollider>().enabled = false;
        isAlive = false;
        enemyAnimator.SetTrigger("Dead");        
    }
}
