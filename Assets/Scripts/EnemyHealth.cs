using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3; // Vida máxima del enemigo
    private int currentHealth; // Vida actual del enemigo

    private void Start()
    {
        // Inicializar la vida del enemigo
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Enemigo " + gameObject.name + " recibe daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemigo " + gameObject.name + " ha muerto.");
        Destroy(gameObject); // Destruye el enemigo
    }
}
