using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatController : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 5f;
    public float attackAngle = 45f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    [SerializeField] private Animator anim;
    public int comboCount = 3; // Número de veces que se repetirá el ataque
    public float attackSpeedIncrease = 0.1f; // Incremento de velocidad por ataque
    public ScoreManager scoreManager; // Referencia al ScoreManager

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E))
        {
            // Verificar si se pueden usar los 30 puntos de tijeras para el ataque especial
            if (scoreManager.CanUseSpecialTijerasAttack())
            {
                // Consumir los puntos y ejecutar el ataque mejorado
                scoreManager.UseSpecialTijerasAttack();
                StartCoroutine(ComboAttack());
            }
            else
            {
                Debug.Log("No tienes suficientes puntos para el ataque especial de tijeras.");
            }
        }
    }

    private IEnumerator ComboAttack()
    {
        for (int i = 0; i < comboCount; i++)
        {
            // Acelerar la animación en cada ataque
            anim.speed = 1 + (i * attackSpeedIncrease);

            // Ejecutar el ataque
            Attack();

            // Esperar a que la animación termine antes de continuar con el siguiente ataque
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / anim.speed);
        }

        // Restaurar la velocidad normal de la animación
        anim.speed = 1;
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Vector3 directionToEnemy = (enemy.transform.position - attackPoint.position).normalized;
            float angleToEnemy = Vector3.Angle(attackPoint.forward, directionToEnemy);

            if (angleToEnemy < attackAngle / 2f)
            {
                Debug.Log("Golpeó a " + enemy.name);
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage, "Tijeras");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i <= 10; i++)
        {
            float currentAngle = Mathf.Lerp(-attackAngle / 2f, attackAngle / 2f, i / 10f);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * attackPoint.forward;
            Gizmos.DrawRay(attackPoint.position, direction * attackRange);
        }
    }
}
