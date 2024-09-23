using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    // Referencia al sistema de puntuación
    public ScoreManager scoreManager;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Método que recibe el daño y el tipo de ataque
    public void TakeDamage(int damage, string attackType)
    {
        currentHealth -= damage;

        Debug.Log("Enemigo " + gameObject.name + " recibe daño por " + attackType + ". Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(attackType);
        }
    }

    private void Die(string attackType)
    {
        Debug.Log("Enemigo " + gameObject.name + " ha muerto por " + attackType);

        // Actualiza la puntuación según el tipo de ataque
        if (scoreManager != null)
        {
            scoreManager.UpdateScore(attackType);
        }

        Destroy(gameObject);
    }
}
