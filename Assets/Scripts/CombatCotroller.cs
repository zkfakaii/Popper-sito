using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Transform attackPoint; // Punto de origen del ataque
    public Vector3 boxSize = new Vector3(2f, 1f, 5f); // Tama�o del box
    public int attackDamage = 1; // Da�o que inflige el ataque
    public LayerMask enemyLayers; // Capa de los enemigos
    [SerializeField] private Animator anim;
    public float attackSpeedMultiplier = 1.2f; // Factor de aumento de velocidad para el ataque especial
    public float baseAttackSpeed = 1f; // Velocidad de ataque base

    private bool isAttacking = false; // Para evitar m�ltiples ataques mientras la animaci�n est� activa
    private bool isSpecialAttack = false; // Para controlar si se est� ejecutando el ataque especial
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        // Ataque normal
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking && !isSpecialAttack)
        {
            StartCoroutine(PerformAttack(baseAttackSpeed));
        }

        // Ataque especial con Shift + E
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E) && scoreManager.CanUseSpecialTijerasAttack() && !isAttacking)
        {
            isSpecialAttack = true;
            scoreManager.UseSpecialTijerasAttack();
            StartCoroutine(PerformSpecialAttack());
        }
    }

    private IEnumerator PerformAttack(float speed)
    {
        isAttacking = true;

        // Inicia la animaci�n de ataque con velocidad ajustada
        anim.SetBool("Corando", true);
        anim.speed = speed;

        // Ejecuta el ataque
        Attack();

        // Espera el tiempo ajustado por la velocidad de la animaci�n
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / speed);

        // Finaliza la animaci�n
        anim.SetBool("Corando", false);
        anim.speed = 1f; // Resetea la velocidad de la animaci�n
        isAttacking = false;
    }

    private IEnumerator PerformSpecialAttack()
    {
        // El ataque especial se ejecuta tres veces, aumentando la velocidad en cada repetici�n
        for (int i = 0; i < 3; i++)
        {
            float speed = baseAttackSpeed * Mathf.Pow(attackSpeedMultiplier, i); // Aumenta la velocidad con cada ataque
            yield return StartCoroutine(PerformAttack(speed));
        }
        isSpecialAttack = false; // Restablece la bandera despu�s de realizar el ataque especial
    }

    private void Attack()
    {
        // Usar BoxCast para detectar enemigos dentro del rango definido por la caja
        Collider[] hitEnemies = Physics.OverlapBox(attackPoint.position, boxSize / 2, attackPoint.rotation, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Golpe� a " + enemy.name);
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage, "CombatController");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Dibuja la caja de ataque en el editor
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(attackPoint.position, attackPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
