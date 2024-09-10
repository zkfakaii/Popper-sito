using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Cherry : MonoBehaviour
{
    public float attackSpeed = 1f; // Velocidad de ataque (tiempo entre ataques)
    public float attackDistance = 5f; // Distancia máxima del ataque
    public float attackDelay = 0.5f; // Retraso antes de lanzar el ataque
    public float raycastLength = 10f; // Longitud del raycast
    public float raycastWidth = 1f; // Anchura del raycast
    public Transform attackPoint; // Punto desde donde se generará el ataque
    public LayerMask playerLayer; // Capa para detectar al jugador
    public float stopDuration = 1f; // Tiempo que el enemigo permanecerá quieto durante el ataque
    public float detectionRange = 20f; // Rango de detección del jugador
    public float rotationSpeed = 10f; // Velocidad de rotación hacia el jugador

    private Transform player; // Referencia al jugador
    private NavMeshAgent navMeshAgent; // Referencia al NavMeshAgent
    private float nextAttackTime = 0f; // Tiempo en el que se puede atacar de nuevo
    private bool isPlayerDetected = false; // Indica si el jugador ha sido detectado

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player no encontrado. Asegúrate de que el objeto del jugador tenga el tag 'Player'.");
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado. Asegúrate de que el objeto tenga un componente NavMeshAgent.");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Detecta al jugador dentro del rango de detección
            if (distanceToPlayer <= detectionRange)
            {
                isPlayerDetected = true;
            }
            else
            {
                isPlayerDetected = false;
            }

            // Si el jugador está dentro del rango de detección, moverse hacia él
            if (isPlayerDetected && distanceToPlayer > attackDistance)
            {
                MoveTowardsPlayer();
            }

            // Si el jugador está dentro del rango de ataque y el tiempo de ataque está listo, atacar
            if (isPlayerDetected && distanceToPlayer <= attackDistance && Time.time >= nextAttackTime)
            {
                StartCoroutine(DelayedAttack());
                nextAttackTime = Time.time + 1f / attackSpeed; // Tiempo hasta el próximo ataque
            }

            // Rotar para mirar al jugador mientras se mueve
            RotateTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Configura el destino del NavMeshAgent hacia la posición del jugador
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.isStopped = false; // Asegurarse de que el agente no esté detenido
    }

    private IEnumerator DelayedAttack()
    {
        // Detén el movimiento del enemigo
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(attackDelay); // Espera el tiempo del retraso

        // Realiza el ataque con BoxCast
        PerformBoxCastAttack();

        // Espera un tiempo adicional después del ataque antes de permitir que el enemigo se mueva nuevamente
        yield return new WaitForSeconds(stopDuration);

        // Reactiva el movimiento del enemigo
        navMeshAgent.isStopped = false;
    }

    private void PerformBoxCastAttack()
    {
        // Define la dirección del BoxCast en función de la dirección actual del enemigo
        Vector3 direction = navMeshAgent.velocity.normalized;
        if (direction == Vector3.zero)
        {
            direction = transform.forward; // Si no se está moviendo, usar la dirección hacia adelante
        }

        // Ajusta el centro y las dimensiones del BoxCast
        Vector3 halfExtents = new Vector3(raycastWidth / 2, raycastWidth / 2, raycastLength / 2);
        Vector3 boxCastCenter = attackPoint.position + direction * (raycastLength / 2);

        RaycastHit hit;
        if (Physics.BoxCast(attackPoint.position, halfExtents, direction, out hit, Quaternion.identity, raycastLength, playerLayer))
        {
            Debug.Log("Golpeó a " + hit.collider.name);
            if (hit.collider.CompareTag("Player"))
            {
                // Aplica el daño al jugador
                hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(1); // Asegúrate de tener un sistema de salud en el jugador
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Establece la rotación hacia la dirección del jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        // Rotar suavemente hacia el jugador
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiza el rango de detección en el editor de Unity
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualiza el tamaño del BoxCast del ataque en el editor
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = attackPoint.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.forward * (raycastLength / 2), new Vector3(raycastWidth, raycastWidth, raycastLength));
        }
    }
}
