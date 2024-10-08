using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public Transform orientation;

    public float dampingFactor = 0.9f;

    public bool gravity = true;

    float horizontalIn;
    float verticalIn;
    float verticalMovementIn;

    Vector3 moveDir;

    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void MyInput() {
        horizontalIn = Input.GetAxisRaw("Horizontal");
        verticalIn = Input.GetAxisRaw("Vertical");
        verticalMovementIn = 0f;
        if(gravity == false) {
            if (Input.GetKey(KeyCode.Q))
            {
                verticalMovementIn = 1f;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                verticalMovementIn = -1f;
            }
        }
    }

    void movePlayer() {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = rb.velocity * dampingFactor;
            return;
        }

        moveDir = orientation.forward * verticalIn + orientation.right * horizontalIn + orientation.up * verticalMovementIn;
        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void Update()
    {
        MyInput();
        if(gravity == false) {
        rb.useGravity = false;
        } else {
            rb.useGravity = true;
        }
    }
    
    void FixedUpdate() {
        movePlayer();
    }
}
