using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontalMoveInput;
    float verticalMoveInput;
    float shootInput;

    Camera cam;
    Vector3 lookTarget;

    [Header("References")]
    [SerializeField] Weapon weapon;

    [Header("Player Attributes")]
    public float playerMaxHealth;
    public float playerCurrentHealth;
    [SerializeField] float playerMoveSpeed;
    [SerializeField] Animator soldierAnim;
    Rigidbody rb;

    [Header("Weapon 1 Attributes")]
    [SerializeField] float weapon1Cooldown;
    float shootTimer;

    [Header("Weapon 2 Attribues")]
    [SerializeField] float weapon2Cooldown;
    public int bulletLeft;

    [Header("Status")]
    bool isMoving;
    bool isAlive;
    bool weapon1Active;
    bool weapon2Active;

    [Header("Effects")]
    [SerializeField] private GameObject muzzleFlash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        shootTimer = 0;
        isMoving = false;
        isAlive = true;
        weapon1Active = true;
        weapon2Active = false;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        PlayerLookAt();
        Shoot();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InputHandler()
    {
        horizontalMoveInput = Input.GetAxis("Horizontal");
        verticalMoveInput = Input.GetAxis("Vertical");

        if (horizontalMoveInput != 0 || verticalMoveInput != 0 && !isMoving && isAlive)
        {
            isMoving = true;
        }
        else isMoving = false;

        soldierAnim.SetFloat("Walking", Mathf.Abs(horizontalMoveInput) + Mathf.Abs(verticalMoveInput));

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapon2Active)
        {
            weapon1Active = true;
            weapon2Active = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapon1Active)
        {
            weapon1Active = false;
            weapon2Active = true;
        }
    }

    private void Shoot()
    {
        shootInput = Input.GetAxis("Fire1");

        if (shootInput != 0 && shootTimer <= Time.time)
        {
            soldierAnim.SetBool("Shoot", true);
            if (weapon1Active && !weapon2Active)
            {
                shootTimer = Time.time + weapon1Cooldown;
                weapon.NormalWeaponMode();
            } 
            else if (weapon2Active && !weapon1Active && bulletLeft > 0)
            {
                shootTimer = Time.time + weapon2Cooldown;
                bulletLeft -= 1;
                weapon.SpreadMode();
            }           
        }
        else
        {
            muzzleFlash.SetActive(false);
            soldierAnim.SetBool("Shoot", false);
        }
    }

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
        isAlive = false;
    }
}
