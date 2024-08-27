using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class CarrotPunch : MonoBehaviour

{
    public GameObject attackPrefab; // Prefab del ataque
    public float attackSpeed = 1f; // Velocidad de ataque (tiempo entre ataques)
    public float attackDistance = 5f; // Distancia a la que se generará el ataque
    public float attackDelay = 0.5f; // Retraso antes de lanzar el ataque
    public Transform attackPoint; // Punto desde donde se generará el ataque
    public LayerMask playerLayer; // Capa para detectar al jugador

    private Transform player; // Referencia al jugador
    private float nextAttackTime = 0f; // Tiempo en el que se puede atacar de nuevo

    private void Start()
    {
        // Encuentra al jugador en la escena por su tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player no encontrado. Asegúrate de que el objeto del jugador tenga el tag 'Player'.");
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
        yield return new WaitForSeconds(attackDelay); // Espera el tiempo del retraso

        // Genera el ataque
        GenerateAttack();
    }

    private void GenerateAttack()
    {
        if (attackPrefab != null && attackPoint != null)
        {
            // Genera el prefab del ataque
            GameObject attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
            // Mueve el ataque hacia adelante
            Rigidbody rb = attack.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(attackPoint.forward * 10f, ForceMode.Impulse); // Ajusta la fuerza según sea necesario
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            // Muestra la distancia de ataque en la escena con un gizmo
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}
