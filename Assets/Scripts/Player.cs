using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    float horizontalMoveInput;
    float verticalMoveInput;
    float shootInput;

    Camera cam;
    Vector3 lookTarget;

    [Header("References")]
    [SerializeField] Weapon weapon;
    [SerializeField] MainLevelUI levelUI;

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
    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool isPausing;
    bool weapon1Active;
    bool weapon2Active;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI bulletDisplay;

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
        shootTimer = Time.time;
        isMoving = false;
        isAlive = true;
        isPausing = false;
        weapon1Active = true;
        weapon2Active = false;

        Time.timeScale = 1.0f;

        levelUI.SetMaxHealth(playerMaxHealth);
        levelUI.CurrentHealth(playerCurrentHealth);
        bulletDisplay.SetText(Mathf.Infinity.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && !isPausing)
        {
            InputHandler();
            PlayerLookAt();
            Shoot();
        }         
    }

    private void FixedUpdate()
    {
        Move();
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

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapon2Active)
        {
            bulletDisplay.SetText(Mathf.Infinity.ToString());
            weapon1Active = true;
            weapon2Active = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapon1Active)
        {
            bulletDisplay.SetText(bulletLeft.ToString());
            weapon1Active = false;
            weapon2Active = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPausing)
        {
            levelUI.PausePanelOn();
        } 
        else if (Input.GetKeyDown(KeyCode.Escape) && isPausing)
        {
            levelUI.PausePanelOff();
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
                bulletDisplay.SetText(bulletLeft.ToString());
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
        levelUI.CurrentHealth(playerCurrentHealth);

        if (playerCurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            levelUI.CurrentHealth(playerCurrentHealth);
            if (weapon2Active)
            {
                bulletDisplay.SetText(bulletLeft.ToString());
            }
        }
    }
}
