using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References
    private Rigidbody rb; // Usaremos un Rigidbody para el movimiento

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
    bool finishedRoitation = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Desactiva la rotación automática del Rigidbody para manejarla manualmente
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        Movement();
    }

    private void Movement()
    {
        GroundMovement();
        RotatePlayer(); // Añadir rotación en la dirección del movimiento
    }

    private void GroundMovement()
    {
        // Cálculo del movimiento en el plano horizontal
        Vector3 move = new Vector3(turnInput, 0, moveInput) * walkSpeed;

        // Aplicar movimiento usando Rigidbody
        if (finishedRoitation) {
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }
    }

    private void RotatePlayer()
    {
        Vector3 moveDirection = new Vector3(turnInput, 0, moveInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            finishedRoitation = (toRotation == transform.rotation);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private bool IsGrounded()
    {
        // Raycast para verificar si el jugador está tocando el suelo
        return Physics.Raycast(transform.position, Vector3.down, 1.1f); // Ajusta la distancia según el tamaño del jugador
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
}
