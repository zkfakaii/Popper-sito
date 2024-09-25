using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // Vida máxima del jugador
    private int currentHealth;  // Vida actual del jugador
    public Image vida; // Imagen que representa la vida
    public List<Sprite> vidasp; // Lista de sprites para las diferentes vidas

    [Header("Scene Settings")]
    public string sceneToLoadOnDeath = "GameOver"; // Nombre de la escena a cargar cuando el jugador muere

    [Header("Invulnerability Settings")]
    public float invulnerabilityDuration = 2f; // Tiempo de invulnerabilidad tras recibir daño
    private bool isInvulnerable = false; // Estado de invulnerabilidad

    public float blinkInterval = 0.1f; // Intervalo de parpadeo durante la invulnerabilidad
    private Renderer[] playerRenderers; // Referencia a los Renderers del jugador y sus hijos

    private void Start()
    {
        // Inicializa la vida actual al máximo al inicio
        currentHealth = maxHealth;
        UpdateUI(); // Actualiza la interfaz de usuario al inicio

        // Obtiene todos los Renderers del jugador y sus hijos para el efecto de parpadeo
        playerRenderers = GetComponentsInChildren<Renderer>();
        if (playerRenderers.Length == 0)
        {
            Debug.LogError("No se encontraron Renderers en el jugador ni en sus hijos.");
        }
    }


    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            // Reduce la vida actual por el daño recibido
            currentHealth -= damage;

            // Asegúrate de que la vida no sea menor que cero
            if (currentHealth <= 0)
            {
                currentHealth = 0; // Asegúrate de no tener vida negativa
                Die();
            }
            else
            {
                // Inicia la corrutina de invulnerabilidad y parpadeo
                StartCoroutine(InvulnerabilityCoroutine());
            }

            UpdateUI(); // Actualiza la UI después de recibir daño
        }
    }

    private void Die()
    {
        // Lógica cuando el jugador muere
        Debug.Log("El jugador ha muerto.");

        // Cambia a la escena deseada
        SceneManager.LoadScene(sceneToLoadOnDeath); // Usa el nombre de la escena desde el Inspector
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        Debug.Log("Jugador es invulnerable por " + invulnerabilityDuration + " segundos.");

        float elapsedTime = 0f;

        while (elapsedTime < invulnerabilityDuration)
        {
            // Alterna la visibilidad de todos los Renderers (jugador y sus hijos)
            foreach (Renderer rend in playerRenderers)
            {
                rend.enabled = !rend.enabled;
            }

            // Espera un corto intervalo antes de volver a alternar
            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval;
        }

        // Asegúrate de que todos los Renderers estén visibles al final del estado de invulnerabilidad
        foreach (Renderer rend in playerRenderers)
        {
            rend.enabled = true;
        }

        isInvulnerable = false;
        Debug.Log("Jugador ya no es invulnerable.");
    }

    private void UpdateUI()
    {
        // Actualiza la imagen de la vida según la salud actual
        if (currentHealth > 4)
        {
            vida.sprite = vidasp[0];
        }
        else if (currentHealth > 3)
        {
            vida.sprite = vidasp[1];
        }
        else if (currentHealth > 2)
        {
            vida.sprite = vidasp[2];
        }
        else if (currentHealth > 1)
        {
            vida.sprite = vidasp[3];
        }
        else
        {
            vida.sprite = vidasp[4];
        }
    }
}
