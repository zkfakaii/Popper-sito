using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Transform attackPoint; // Punto de origen del ataque
    public float attackRange = 1f; // Rango del ataque
    public int attackDamage = 1; // Da�o que inflige el ataque
    public LayerMask enemyLayers; // Capa de los enemigos

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Dibuja un Gizmo que representa el �rea de ataque
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Aplica da�o a cada enemigo en el �rea de ataque
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Golpe� a " + enemy.name);
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Dibuja el rango de ataque en el editor de Unity
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
