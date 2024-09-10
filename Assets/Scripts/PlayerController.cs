using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References
    [Header("References")]
    private CharacterController controller;

    // Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float rotationSpeed = 10f; // Velocidad de rotación

    private float verticalVelocity;

    // Input
    [Header("Input")]
    private float moveInput;
    private float turnInput;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        Movement();
        Debug.Log(controller.velocity);
    }

    private void Movement()
    {
        GroundMovement();
        RotatePlayer(); // Añadir rotación en la dirección del movimiento
    }

    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move.y = 0;
        move *= walkSpeed;

        move.y = VerticalForceCalculation();

        //controller.Move(move * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        Vector3 moveDirection = new Vector3(turnInput, 0, moveInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
}
