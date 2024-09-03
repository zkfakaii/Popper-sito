using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Celery : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde se disparan los proyectiles
    public float fireRate = 2f; // Tiempo entre disparos
    public float projectileSpeed = 10f; // Velocidad del proyectil
    public float attackRange = 15f; // Distancia máxima para atacar
    public float detectionRange = 20f; // Rango de detección del jugador
    public float dodgeSpeed = 5f; // Velocidad de movimiento al esquivar
    public float dodgeTime = 1f; // Tiempo que se mueve en el eje Z al detectar al jugador

    private Transform player; // Referencia al jugador
    private NavMeshAgent navMeshAgent; // Referencia al NavMeshAgent
    private bool canShoot = true; // Controla si el enemigo puede disparar
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
            if (isPlayerDetected && distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }

            // Si el jugador está dentro del rango de ataque, detenerse y disparar
            if (isPlayerDetected && distanceToPlayer <= attackRange && canShoot)
            {
                navMeshAgent.isStopped = true; // Detener el movimiento del agente
                StartCoroutine(MoveAndShoot());
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // Configura el destino del NavMeshAgent hacia la posición del jugador
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.isStopped = false; // Asegurarse de que el agente no esté detenido
    }

    private IEnumerator MoveAndShoot()
    {
        canShoot = false;

        // Mueve al enemigo en el eje Z durante dodgeTime utilizando NavMeshAgent
        Vector3 dodgeDirection = transform.forward * dodgeSpeed;
        navMeshAgent.Move(dodgeDirection * Time.deltaTime);

        yield return new WaitForSeconds(dodgeTime);

        // Instancia y dispara el proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (player.position - firePoint.position).normalized * projectileSpeed;

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        navMeshAgent.isStopped = false; // Reanudar el movimiento después de disparar
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiza el rango de detección en el editor de Unity
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualiza el rango de ataque en el editor de Unity
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
