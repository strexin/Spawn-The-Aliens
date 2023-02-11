using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    bool isMoving;
    [SerializeField] float speedMove;

    float horizontalMoveInput;
    float verticalMoveInput;

    Camera cam;
    Vector3 lookTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

    }

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        PlayerLookAt();

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
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (isMoving)
        {
            if (horizontalMoveInput != 0)
            {
                rb.velocity = Vector3.right * horizontalMoveInput * speedMove;
            }
            
            if (verticalMoveInput != 0)
            {
                rb.velocity = Vector3.forward * verticalMoveInput * speedMove;
            }
        }
    }
}
