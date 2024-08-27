using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarrotPunch : MonoBehaviour

{
    public GameObject attackPrefab; // Prefab del ataque
    public float attackSpeed = 1f; // Velocidad de ataque (tiempo entre ataques)
    public float attackDelay = 0.5f; // Tiempo antes de que el ataque sea generado
    public Transform attackPoint; // Punto desde donde se generará el ataque

    private float nextAttackTime = 0f; // Tiempo en el que se puede atacar de nuevo

    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            // Genera el ataque después de un cierto retraso
            Invoke("GenerateAttack", attackDelay);
            nextAttackTime = Time.time + 1f / attackSpeed; // Tiempo hasta el próximo ataque
        }
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
}
