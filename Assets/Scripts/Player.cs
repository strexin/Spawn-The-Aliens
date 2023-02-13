using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    float horizontalMoveInput;
    float verticalMoveInput;
    float shootInput;

    Camera cam;
    Vector3 lookTarget;
    GameObject enemy;

    [Header("References")]

    [Header("Player Attributes")]
    [SerializeField] float playerMaxHealth;
    [SerializeField] float playerCurrentHealth;
    [SerializeField] float playerMoveSpeed;
    [SerializeField] Animator soldierAnim;
    Rigidbody rb;

    [Header("Weapon 1 Attributes")]
    [SerializeField] float weapon1Damage;
    [SerializeField] float shootCooldown;
    float shootTimer;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float shootMaxRange;

    [Header("Weapon 2 Attribues")]
    [SerializeField] float weapon2Damage;

    [Header("Status")]
    bool isMoving;

    [Header("Effects")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private HitEffect hitEffect;
    private IObjectPool<HitEffect> hitPool;
    private Vector3 hitPos;

    private void Awake()
    {
        hitPool = new ObjectPool<HitEffect>(CreateHitEffect, OnGet, OnRelease, OnDestroyClone, maxSize:20);
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        shootTimer = 0;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        PlayerLookAt();
        Shoot();
    }

    private void Shoot()
    {
        shootInput = Input.GetAxis("Fire1");

        if (shootInput != 0 && shootTimer <= Time.time)
        {
            shootTimer = Time.time + shootCooldown;
            CreateRayToShoot();
        }
        else
        {
            muzzleFlash.SetActive(false);
            soldierAnim.SetBool("Shoot", false);
        }
    }

    private void CreateRayToShoot()
    {
        Ray ray = new Ray(muzzlePoint.position, transform.TransformDirection(Vector3.forward));
        RaycastHit hit;
        muzzleFlash.SetActive(true);
        soldierAnim.SetBool("Shoot", true);

        if (Physics.Raycast(ray, out hit, shootMaxRange))
        {
            hitPos = hit.point;
            hitPool.Get();
            if (hit.collider.tag == "Enemy")
            {
                enemy = hit.collider.gameObject;
                enemy.GetComponent<Enemy>().GetHit(weapon1Damage);
            }
        }
    }

    #region Hit Effect Pooling
    private HitEffect CreateHitEffect()
    {
        HitEffect effect = Instantiate(hitEffect);
        effect.SetPool(hitPool);
        return effect;
    }
    private void OnGet(HitEffect effect)
    {
        effect.gameObject.SetActive(true);
        effect.transform.position = hitPos;
    }

    private void OnRelease(HitEffect effect)
    {
        effect.gameObject.SetActive(false);
    }

    private void OnDestroyClone(HitEffect effect)
    {
        Destroy(effect.gameObject);
    }
    #endregion

    private void PlayerLookAt()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            lookTarget = hit.point;
        }

        var lookPos = lookTarget - transform.position;
        lookPos.y = 0.0f;
        if (lookPos != Vector3.zero)
        {
            transform.LookAt(transform.position + lookPos, Vector3.up);
        }
    }

    private void InputHandler()
    {
        horizontalMoveInput = Input.GetAxis("Horizontal");
        verticalMoveInput = Input.GetAxis("Vertical");

        if (horizontalMoveInput != 0 || verticalMoveInput != 0 && !isMoving)
        {
            isMoving = true;
        }
        else isMoving = false;

        soldierAnim.SetFloat("Walking", Mathf.Abs(horizontalMoveInput) + Mathf.Abs(verticalMoveInput));
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (isMoving)
        {
            Vector3 movement = new Vector3(horizontalMoveInput, 0.0f, verticalMoveInput);
            rb.velocity = movement * playerMoveSpeed;
        }
    }

    public void GetHit(float damage)
    {
        playerCurrentHealth -= damage;
        if (playerCurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");
    }
}
