
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrotPunch : MonoBehaviour
{
    public float attackSpeed = 1f; // Velocidad de ataque (tiempo entre ataques)
    public float attackDistance = 5f; // Distancia máxima del ataque
    public float attackDelay = 0.5f; // Retraso antes de lanzar el ataque
    public float raycastLength = 10f; // Longitud del raycast
    public float raycastWidth = 1f; // Anchura del raycast
    public Transform attackPoint; // Punto desde donde se generará el ataque
    public LayerMask playerLayer; // Capa para detectar al jugador
    public float stopDuration = 1f; // Tiempo que el enemigo permanecerá quieto durante el ataque

    private Transform player; // Referencia al jugador
    private float nextAttackTime = 0f; // Tiempo en el que se puede atacar de nuevo
    private NavMeshAgent navMeshAgent; // Referencia al NavMeshAgent para movimiento

    private void Start()
    {
        // Encuentra al jugador en la escena por su tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player no encontrado. Asegúrate de que el objeto del jugador tenga el tag 'Player'.");
        }

        // Obtén el NavMeshAgent para controlar el movimiento
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado.");
        }
    }

    private void Update()
    {
        if (player != null && Time.time >= nextAttackTime)
        {
            // Verifica la distancia entre el enemigo y el jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackDistance)
            {
                // Inicia la coroutine para lanzar el ataque con un retraso
                StartCoroutine(DelayedAttack());
                nextAttackTime = Time.time + 1f / attackSpeed; // Tiempo hasta el próximo ataque
            }
        }
    }

    private IEnumerator DelayedAttack()
    {
        // Detén el movimiento del enemigo
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }

        yield return new WaitForSeconds(attackDelay); // Espera el tiempo del retraso

        // Realiza el ataque con Raycast
        PerformRaycastAttack();

        // Espera un tiempo adicional después del ataque antes de permitir que el enemigo se mueva nuevamente
        yield return new WaitForSeconds(stopDuration);

        // Reactiva el movimiento del enemigo
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
        }
    }

    private void PerformRaycastAttack()
    {
        // Define la dirección del raycast en función de la dirección actual del movimiento del enemigo
        Vector3 direction = navMeshAgent.velocity.normalized;
        if (direction == Vector3.zero)
        {
            direction = transform.forward; // Si no se está moviendo, usar la dirección hacia adelante
        }

        // Crea un raycast de tipo caja (BoxCast) con dimensiones ajustables
        RaycastHit[] hits = Physics.BoxCastAll(
            attackPoint.position,
            new Vector3(raycastWidth / 2, raycastWidth / 2, raycastLength / 2),
            direction,
            attackPoint.rotation,
            raycastLength,
            playerLayer);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Debug.Log("Golpeó a " + hit.collider.name);
                // Aplica el daño al jugador
                hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(1); // Asegúrate de tener un sistema de salud en el jugador
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            // Visualiza el tamaño del BoxCast en el editor
            Gizmos.color = Color.red;
            Gizmos.matrix = attackPoint.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.forward * (raycastLength / 2), new Vector3(raycastWidth, raycastWidth, raycastLength));
        }
    }
}
