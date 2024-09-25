using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Importante para usar NavMeshAgent

public class Carrot : MonoBehaviour
{
    public float speed = 3f; // Velocidad de movimiento del enemigo
    private Transform player; // Referencia al jugador
    private bool hasCollided = false; // Indica si ya ha colisionado con el jugador
    private NavMeshAgent navMeshAgent; // Referencia al componente NavMeshAgent
    public float rotationSpeed = 5f; // Velocidad de rotación para mirar al jugador

    private void Start()
    {
        // Encuentra al jugador en la escena por su tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player no encontrado. Asegúrate de que el objeto del jugador tenga el tag 'Player'.");
        }

        // Obtiene el componente NavMeshAgent del enemigo
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado. Asegúrate de que el objeto tenga el componente NavMeshAgent.");
        }
        else
        {
            navMeshAgent.speed = speed; // Configura la velocidad del NavMeshAgent
        }
    }

    private void Update()
    {
        if (player != null && !hasCollided)
        {
            // Mueve el enemigo hacia el jugador usando NavMeshAgent
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Configura el destino del NavMeshAgent hacia el jugador
        navMeshAgent.SetDestination(player.position);

        // Gira al enemigo para que mire hacia el jugador
        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Crea una rotación para mirar en la dirección del jugador
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Aplica una rotación suave hacia el jugador
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

 
    
}
