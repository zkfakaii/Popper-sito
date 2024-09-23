using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private ScoreManager scoreManager;

    private void Start()
    {
        currentHealth = maxHealth;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void TakeDamage(int damage, string attackType)
    {
        currentHealth -= damage;

        Debug.Log("Enemigo " + gameObject.name + " recibe daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(attackType);
        }
    }

    private void Die(string attackType)
    {
        Debug.Log("Enemigo " + gameObject.name + " ha muerto.");

        // Sumar puntos basados en el tipo de ataque
        scoreManager.AddPoints(attackType, 10); // Aquí decides cuántos puntos se otorgan por cada muerte

        Destroy(gameObject);
    }
}
