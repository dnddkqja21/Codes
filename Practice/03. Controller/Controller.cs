using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    CharacterController characterController;
    float speed = 5f;
    float jumpForce = 8.0f;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical);
        Vector3 moveVector = moveDirection * speed;

        characterController.Move(moveVector * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            Debug.Log("충돌 확인");
        }

        if(other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2.0f * Physics.gravity.y);
        isGrounded = false;
    }
}
