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
    [SerializeField] private float jumpForce = 5f; // Fuerza del salto
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float rotationSpeed = 10f; // Velocidad de rotación

    private float verticalVelocity;
    private bool isJumping = false; // Para verificar si está saltando

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
        HandleJump(); // Manejar el salto
    }

    private void GroundMovement()
    {
        // Cálculo del movimiento en el plano horizontal
        Vector3 move = new Vector3(turnInput, 0, moveInput) * walkSpeed;

        // Aplicar movimiento usando Rigidbody
        if (finishedRoitation)
        {
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }


    }

    private void ApplyGravity()
    {
        if (!IsGrounded())
        {
            rb.AddForce(Vector3.down * gravity * rb.mass);
            Debug.Log("Aplicando gravedad al jugador");
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
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // Inicia el raycast un poco por encima del jugador
        bool grounded = Physics.Raycast(rayStart, Vector3.down, 1.3f); // Ajusta la distancia según el tamaño del jugador
        //Debug.Log(grounded ? "Jugador está en el suelo" : "Jugador NO está en el suelo");
        return grounded;

       
    }

    private void HandleJump()
    {
        // Detectar si se pulsa la tecla de salto y si el jugador está en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !isJumping)
        {
            // Aplicar fuerza de salto en el eje Y
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true; // Marcar que está saltando

            // Debug para indicar que el jugador ha saltado
           // Debug.Log("Jugador ha saltado con una fuerza de: " + jumpForce);
        }

        // Verificar si ha aterrizado
        if (IsGrounded() && rb.velocity.y <= 0)
        {
            isJumping = false; // Restablecer el estado de salto

            // Debug para indicar que el jugador ha aterrizado
           // Debug.Log("Jugador ha aterrizado.");//
        }
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
}


