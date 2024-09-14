using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Transform attackPoint; // Punto de origen del ataque
    public float attackRange = 5f; // Distancia m�xima del ataque
    public float attackAngle = 45f; // �ngulo de apertura del cono
    public int attackDamage = 1; // Da�o que inflige el ataque
    public LayerMask enemyLayers; // Capa de los enemigos
    [SerializeField] private Animator anim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Activa la animaci�n de ataque

            anim.SetBool("Corando", true);

            // Ejecuta el ataque
            Attack();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetBool("Corando", false);

        }
    }

    private void Attack()
    {
        // Obtener todos los colliders en el rango del ataque
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            // Calcular la direcci�n desde el punto de ataque al enemigo
            Vector3 directionToEnemy = (enemy.transform.position - attackPoint.position).normalized;

            // Calcular el �ngulo entre la direcci�n de ataque (hacia adelante) y la direcci�n al enemigo
            float angleToEnemy = Vector3.Angle(attackPoint.forward, directionToEnemy);

            // Si el enemigo est� dentro del �ngulo de ataque, aplicarle da�o
            if (angleToEnemy < attackAngle / 2f)
            {
                Debug.Log("Golpe� a " + enemy.name);
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Dibuja el rango de ataque como un cono
        Gizmos.color = Color.red;

        // Dibuja l�neas radiales para simular el cono en el editor
        for (int i = 0; i <= 10; i++)
        {
            float currentAngle = Mathf.Lerp(-attackAngle / 2f, attackAngle / 2f, i / 10f);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * attackPoint.forward;
            Gizmos.DrawRay(attackPoint.position, direction * attackRange);
        }
    }
}
