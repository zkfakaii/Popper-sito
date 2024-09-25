
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
    private bool isAttacking = false; // Nueva variable para controlar el estado del ataque

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        // Solo permitir que se inicie un ataque si no est� atacando
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            StartCoroutine(PerformAttackWithAnimation());
        }

        // Verifica si se presiona Shift + F para el ataque especial
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F) && !isAttacking && scoreManager.CanUseSpecialPapasAttack())
            Debug.Log("iansolis");
        {
            isSpecialAttack = true;
            scoreManager.UseSpecialPapasAttack();
            StartCoroutine(PerformAttackWithAnimation());
        }
    }

    private IEnumerator PerformAttackWithAnimation()
    {
        isAttacking = true; // Inicia el ataque

        // Activa la animaci�n
        anim.SetBool("saco", true);

        // Espera hasta que la animaci�n se complete
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animInfo.length);

        // Realiza el ataque despu�s de que la animaci�n se haya completado
        StartCoroutine(AttackWithDelay());

        // Espera a que termine la animaci�n de ataque antes de permitir otro ataque
        yield return new WaitForSeconds(animInfo.length);

        // Finaliza la animaci�n
        anim.SetBool("saco", false);

        // Permite que el ataque se pueda ejecutar de nuevo
        isAttacking = false;
    }

    private IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        Attack();
    }

    private void Attack()
    {
        float currentAttackRange = isSpecialAttack ? 8f : attackRange; // Rango especial
        float currentAttackAngle = isSpecialAttack ? 360f : attackAngle; // �ngulo especial

        // Detecta enemigos en el �rea esf�rica
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, currentAttackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            // Calcula la direcci�n hacia el enemigo desde el punto de ataque
            Vector3 directionToEnemy = (enemy.transform.position - attackPoint.position).normalized;

            // Calcula el �ngulo entre el frente del ataque y la direcci�n hacia el enemigo
            float angleToEnemy = Vector3.Angle(attackPoint.forward, directionToEnemy);

            // Si el enemigo est� dentro del �ngulo definido, aplica el da�o
            if (angleToEnemy < currentAttackAngle / 2f)
            {
                Debug.Log("Golpe� a " + enemy.name);
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage, "Papas");
            }
        }

        if (isSpecialAttack)
        {
            isSpecialAttack = false; // Reinicia el estado despu�s del ataque especial
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // Dibuja el rango de ataque (esfera o semiesfera)
        Gizmos.color = isSpecialAttack ? Color.yellow : Color.grey;
        Gizmos.DrawWireSphere(attackPoint.position, isSpecialAttack ? 8f : attackRange);

        // Dibuja el �ngulo del ataque en forma de semiesfera
        for (int i = 0; i <= 10; i++)
        {
            float currentAngle = Mathf.Lerp(-attackAngle / 2f, attackAngle / 2f, i / 10f);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * attackPoint.forward;
            Gizmos.DrawRay(attackPoint.position, direction * attackRange);
        }

        if (FindObjectOfType<ScoreManager>().CanUseSpecialPapasAttack())
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, 8f); // Dibuja el c�rculo completo para el ataque especial
        }
    }
}
