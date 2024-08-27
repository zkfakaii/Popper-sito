using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public float speed = 3f; // Velocidad de movimiento del enemigo
    private Transform player; // Referencia al jugador
    private bool hasCollided = false; // Indica si ya ha colisionado con el jugador

    private void Start()
    {
        // Encuentra al jugador en la escena por su tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player no encontrado. Aseg�rate de que el objeto del jugador tenga el tag 'Player'.");
        }
    }

    private void Update()
    {
        if (player != null && !hasCollided)
        {
            // Mueve el enemigo hacia el jugador
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calcula la direcci�n hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Mueve al enemigo en esa direcci�n
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el enemigo ha colisionado con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // L�gica a ejecutar cuando colisiona con el jugador
            Debug.Log("Carrot ha colisionado con el jugador!");

            // Detenemos el movimiento del enemigo tras la colisi�n
            hasCollided = true;

            // Aqu� puedes agregar cualquier otra l�gica adicional, como reducir la vida del jugador
        }
    }
}
