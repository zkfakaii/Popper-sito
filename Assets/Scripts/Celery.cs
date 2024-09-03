using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Celery : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde se disparan los proyectiles
    public float fireRate = 2f; // Tiempo entre disparos
    public float projectileSpeed = 10f; // Velocidad del proyectil
    public float attackRange = 15f; // Distancia máxima para atacar
    public float detectionRange = 20f; // Rango de detección del jugador
    public float moveSpeed = 3f; // Velocidad de movimiento del enemigo
    public float dodgeSpeed = 5f; // Velocidad de movimiento al esquivar
    public float dodgeTime = 1f; // Tiempo que se mueve en el eje Z al detectar al jugador

    private Transform player; // Referencia al jugador
    private bool canShoot = true; // Controla si el enemigo puede disparar
    private bool isPlayerDetected = false; // Indica si el jugador ha sido detectado

    [SerializeField] Vector3 novDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player no encontrado. Asegúrate de que el objeto del jugador tenga el tag 'Player'.");
        }
    }

    private void FixedUpdate()
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

            // Si el jugador está dentro del rango de ataque, esquivar y disparar
            if (isPlayerDetected && distanceToPlayer <= attackRange)
            {
                if (canShoot)
                {
                    StartCoroutine(Shoot());
                }
               

                StartDodging();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Mueve al enemigo en esa dirección
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        // Instancia y dispara el proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (player.position - firePoint.position).normalized * projectileSpeed;

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void StartDodging()
    {
        // Mueve al enemigo en el eje Z mientras esquiva
        transform.position += novDirection * dodgeSpeed * Time.deltaTime;
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
