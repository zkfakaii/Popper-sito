

using System.Collections;
using UnityEngine;

public class Papas : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 5f;
    public float attackAngle = 45f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public float attackDelay = 0.5f;
    [SerializeField] private Animator anim;

    private ScoreManager scoreManager;
    private bool isSpecialAttack = false;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(AttackWithDelay());
            anim.SetBool("saco", true);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            anim.SetBool("saco", false);
        }

        // Verifica si se presiona Shift + F para el ataque especial
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F) && scoreManager.CanUseSpecialPapasAttack())
        {
            isSpecialAttack = true;
            scoreManager.UseSpecialPapasAttack();
            StartCoroutine(AttackWithDelay());
        }
    }

    private IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        Attack();
    }

    private void Attack()
    {
        float currentAttackRange = isSpecialAttack ? 8f : attackRange; // Rango especial
        float currentAttackAngle = isSpecialAttack ? 360f : attackAngle; // Ángulo especial

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, currentAttackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Vector3 directionToEnemy = (enemy.transform.position - attackPoint.position).normalized;
            float angleToEnemy = Vector3.Angle(attackPoint.forward, directionToEnemy);

            if (angleToEnemy < currentAttackAngle / 2f)
            {
                Debug.Log("Golpeó a " + enemy.name);
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage, "Papas");
            }
        }

        if (isSpecialAttack)
        {
            isSpecialAttack = false; // Reinicia el estado después del ataque especial
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = isSpecialAttack ? Color.yellow : Color.grey;
        for (int i = 0; i <= 10; i++)
        {
            float currentAngle = Mathf.Lerp(-attackAngle / 2f, attackAngle / 2f, i / 10f);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * attackPoint.forward;
            Gizmos.DrawRay(attackPoint.position, direction * attackRange);
        }


      
        if (attackPoint == null)
                return;

            // Dibuja el rango de ataque especial
            Gizmos.color = Color.yellow;

            // Verifica si los puntos de Papas son suficientes para el ataque especial
            if (FindObjectOfType<ScoreManager>().CanUseSpecialPapasAttack())
            {
                // Dibuja el círculo completo con rango de 8 para el ataque especial
                Gizmos.DrawWireSphere(attackPoint.position, 8f);
            }
            else
            {
                // Dibuja el cono normal
                Gizmos.color = Color.grey;
                for (int i = 0; i <= 10; i++)
                {
                    float currentAngle = Mathf.Lerp(-attackAngle / 2f, attackAngle / 2f, i / 10f);
                    Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * attackPoint.forward;
                    Gizmos.DrawRay(attackPoint.position, direction * attackRange);
                }
            }
        

    }

}
