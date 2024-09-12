using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vida máxima del jugador
    private int currentHealth;  // Vida actual del jugador

    [Header("Scene Settings")]
    public string sceneToLoadOnDeath = "GameOver"; // Nombre de la escena a cargar cuando el jugador muere

    [Header("Invulnerability Settings")]
    public float invulnerabilityDuration = 2f; // Tiempo de invulnerabilidad tras recibir daño
    private bool isInvulnerable = false; // Estado de invulnerabilidad

    public float blinkInterval = 0.1f; // Intervalo de parpadeo durante la invulnerabilidad
    private Renderer[] playerRenderers; // Referencia a los Renderer del jugador y sus hijos

    private void Start()
    {
        // Inicializa la vida actual al máximo al inicio
        currentHealth = maxHealth;

        // Obtiene todos los Renderers del jugador y sus hijos para el efecto de parpadeo
        playerRenderers = GetComponentsInChildren<Renderer>();
        if (playerRenderers.Length == 0)
        {
            Debug.LogError("No se encontraron Renderers en el jugador ni en sus hijos.");
        }
    }

    private void OnGUI()
    {
        // Muestra la vida actual en la pantalla usando GUI
        GUIStyle style = new GUIStyle();
        style.fontSize = 64;
        style.normal.textColor = Color.white;
        GUILayout.Label("Vida: " + currentHealth, style);
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
                Die();
            }
            else
            {
                // Inicia la corrutina de invulnerabilidad y parpadeo
                StartCoroutine(InvulnerabilityCoroutine());
            }
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
}
