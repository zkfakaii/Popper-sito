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
    public float stopDuration = 1f; // Tiempo que el enemigo permanecerá quieto durante el ataque

    private Transform player; // Referencia al jugador
    private float nextAttackTime = 0f; // Tiempo en el que se puede atacar de nuevo
    private Carrot carrotMovement; // Referencia al script de movimiento del enemigo

    private void Start()
    {
        // Encuentra al jugador en la escena por su tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player no encontrado. Asegúrate de que el objeto del jugador tenga el tag 'Player'.");
        }

        // Obtén la referencia al script de movimiento
        carrotMovement = GetComponent<Carrot>();
        if (carrotMovement == null)
        {
            Debug.LogError("Script Carrot no encontrado en el objeto enemigo.");
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
        if (carrotMovement != null)
        {
            carrotMovement.enabled = false;
        }

        yield return new WaitForSeconds(attackDelay); // Espera el tiempo del retraso

        // Genera el ataque
        GenerateAttack();

        // Espera un tiempo adicional después del ataque antes de permitir que el enemigo se mueva nuevamente
        yield return new WaitForSeconds(stopDuration);

        // Reactiva el movimiento del enemigo
        if (carrotMovement != null)
        {
            carrotMovement.enabled = true;
        }
    }

    private void GenerateAttack()
    {
        if (attackPrefab != null && attackPoint != null)
        {
            // Genera el prefab del ataque
            GameObject attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);

            // Inicializa el prefab del ataque con el attackPoint para que lo siga
            Puño puñoScript = attack.GetComponent<Puño>();
            if (puñoScript != null)
            {
                puñoScript.Initialize(attackPoint);
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
