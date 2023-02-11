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

    [Header("Player Attribute")]
    [SerializeField] float playerMaxHealth;
    [SerializeField] float playerCurrentHealth;
    [SerializeField] float playerMoveSpeed;
    [SerializeField] Animator soldierAnim;

    Rigidbody rb;

    [Header("Weapon Attribut")]
    [SerializeField] float shootCooldown;
    float shootTimer;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float shootMaxRange;

    [Header("Status")]
    bool isMoving;

    [Header("Effect")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject hitEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

    }

    // Start is called before the first frame update
    void Start()
    {
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

        if (shootInput != 0 && shootTimer < Time.time)
        {
            shootTimer = Time.time + shootCooldown;
            Ray ray = new Ray(muzzlePoint.position, transform.TransformDirection(Vector3.forward));
            RaycastHit hit;
            muzzleFlash.SetActive(true);
            soldierAnim.SetBool("Shoot", true);

            if (Physics.Raycast(ray, out hit, shootMaxRange))
            {
                Destroy(Instantiate(hitEffect, hit.transform.position, Quaternion.identity), 0.5f);               
                Debug.DrawRay(muzzlePoint.position, transform.TransformDirection(Vector3.forward));
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("Hit");
                    //Decrease enemy health
                }
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
        transform.LookAt(transform.position + lookPos, Vector3.up);
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
}
