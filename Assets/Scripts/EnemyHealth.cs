using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    // Referencia al sistema de puntuaci�n
    public ScoreManager scoreManager;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // M�todo que recibe el da�o y el tipo de ataque
    public void TakeDamage(int damage, string attackType)
    {
        currentHealth -= damage;

        Debug.Log("Enemigo " + gameObject.name + " recibe da�o por " + attackType + ". Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(attackType);
        }
    }

    private void Die(string attackType)
    {
        Debug.Log("Enemigo " + gameObject.name + " ha muerto por " + attackType);

        // Actualiza la puntuaci�n seg�n el tipo de ataque
        if (scoreManager != null)
        {
            scoreManager.UpdateScore(attackType);
        }

        Destroy(gameObject);
    }
}
