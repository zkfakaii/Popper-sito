using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Transform attackPoint;
    public Vector3 boxSize = new Vector3(2f, 1f, 5f);
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    [SerializeField] private Animator anim;

    private bool isAttacking = false;
    private bool isSpecialAttack = false; // Variable para identificar si es el ataque especial
    private ScoreManager scoreManager; // Referencia al ScoreManager

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>(); // Obtener el ScoreManager
    }

    private void Update()
    {
        // Ataque normal
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }

        // Ataque especial si se presiona Shift + E y hay suficientes puntos
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E) && !isAttacking && scoreManager.CanUseSpecialTijerasAttack())
        {
            Debug.Log("Preparando ataque especial de Tijeras"); // Debug cuando se detecta la combinación de teclas
            isSpecialAttack = true;
            scoreManager.UseSpecialTijerasAttack();
            StartCoroutine(PerformSpecialAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        Debug.Log("Realizando ataque normal de Tijeras"); // Debug para el ataque normal
        anim.SetBool("Corando", true);
        isAttacking = true;

        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        float animationLength = animInfo.length;

        Attack();

        yield return new WaitForSeconds(animationLength);

        anim.SetBool("Corando", false);
        isAttacking = false;
    }

    private IEnumerator PerformSpecialAttack()
    {
        Debug.Log("Realizando ataque especial de Tijeras"); // Debug al iniciar el ataque especial
        isAttacking = true;
        anim.SetBool("Corando", true);

        for (int i = 0; i < 3; i++) // Realizar el ataque 3 veces
        {
            Debug.Log($"Ataque especial {i + 1} de 3"); // Debug para cada repetición del ataque especial
            Attack();
            yield return new WaitForSeconds(0.5f); // Tiempo entre ataques, puedes ajustarlo según lo que necesites
        }

        anim.SetBool("Corando", false);
        isAttacking = false;
        isSpecialAttack = false; // Resetear el estado de ataque especial
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapBox(attackPoint.position, boxSize / 2, attackPoint.rotation, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Golpeó a " + enemy.name);
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage, isSpecialAttack ? "CombatControllerSpecial" : "CombatController");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(attackPoint.position, attackPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
