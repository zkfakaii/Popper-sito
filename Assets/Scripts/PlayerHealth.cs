using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vida m�xima del jugador
    private int currentHealth;  // Vida actual del jugador

    [Header("Scene Settings")]
    public string sceneToLoadOnDeath = "GameOver"; // Nombre de la escena a cargar cuando el jugador muere

    private void Start()
    {
        // Inicializa la vida actual al m�ximo al inicio
        currentHealth = maxHealth;
    }

    private void OnGUI()
    {
        // Muestra la vida actual en la pantalla usando GUI
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;
        GUILayout.Label("Vida: " + currentHealth, style);
    }

    public void TakeDamage(int damage)
    {
        // Reduce la vida actual por el da�o recibido
        currentHealth -= damage;

        // Aseg�rate de que la vida no sea menor que cero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // L�gica cuando el jugador muere
        Debug.Log("El jugador ha muerto.");

        // Cambia a la escena deseada
        SceneManager.LoadScene(sceneToLoadOnDeath); // Usa el nombre de la escena desde el Inspector
    }
}
