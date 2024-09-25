using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrito : MonoBehaviour
{
    public Transform attackPoint; // Punto de origen del ataque
    public Vector3 boxSize = new Vector3(2f, 1f, 5f); // Tama�o del box
    public int attackDamage = 1; // Da�o que inflige el ataque
    public LayerMask enemyLayers; // Capa de los enemigos
    [SerializeField] private Animator anim;

    private bool isAttacking = false; // Para evitar m�ltiples ataques mientras la animaci�n est� activa

    private void Update()
    {
        // Solo permitir que el jugador ataque si no est� ya atacando
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        // Iniciar la animaci�n de ataque
        anim.SetBool("Carro", true);
        isAttacking = true;

        // Esperar a que la animaci�n comience y obtenga su duraci�n
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        float animationLength = animInfo.length;

        // Ejecuta el ataque
        Attack();

        // Esperar hasta que la animaci�n termine
        yield return new WaitForSeconds(animationLength);

        // Finalizar la animaci�n
        anim.SetBool("Carro", false);
        isAttacking = false;
    }

    private void Attack()
    {
        // Usar BoxCast para detectar enemigos dentro del rango definido por la caja
        Collider[] hitEnemies = Physics.OverlapBox(attackPoint.position, boxSize / 2, attackPoint.rotation, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Golpe� a " + enemy.name);
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage, "Carrito");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Dibuja la caja de ataque en el editor
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(attackPoint.position, attackPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
