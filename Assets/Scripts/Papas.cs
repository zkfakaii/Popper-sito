using System.Collections;
using UnityEngine;

public class Papas : MonoBehaviour
{
    public Transform attackPoint; // Punto de origen del ataque
    public float attackRange = 5f; // Distancia máxima del ataque
    public float attackAngle = 45f; // Ángulo de apertura del cono
    public int attackDamage = 1; // Daño que inflige el ataque
    public LayerMask enemyLayers; // Capa de los enemigos
    public float attackDelay = 0.5f; // Retraso antes de infligir daño
    [SerializeField] private Animator anim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(AttackWithDelay());
            anim.SetBool("saco",true);
        }
        if (Input.GetKeyUp(KeyCode.F)) 
            {
            anim.SetBool("saco", false);

        }

    }

    private IEnumerator AttackWithDelay()
    {
        // Espera antes de realizar el ataque
        yield return new WaitForSeconds(attackDelay);
        Attack();
    }

    private void Attack()
    {
        // Obtener todos los colliders en el rango del ataque
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            // Calcular la dirección desde el punto de ataque al enemigo
            Vector3 directionToEnemy = (enemy.transform.position - attackPoint.position).normalized;

            // Calcular el ángulo entre la dirección de ataque (hacia adelante) y la dirección al enemigo
            float angleToEnemy = Vector3.Angle(attackPoint.forward, directionToEnemy);

            // Si el enemigo está dentro del ángulo de ataque, aplicarle daño
            if (angleToEnemy < attackAngle / 2f)
            {
                Debug.Log("Golpeó a " + enemy.name);
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage, "Papas");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Dibuja el rango de ataque como un cono
        Gizmos.color = Color.grey;

        // Dibuja líneas radiales para simular el cono en el editor
        for (int i = 0; i <= 10; i++)
        {
            float currentAngle = Mathf.Lerp(-attackAngle / 2f, attackAngle / 2f, i / 10f);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * attackPoint.forward;
            Gizmos.DrawRay(attackPoint.position, direction * attackRange);
        }
    }
}
