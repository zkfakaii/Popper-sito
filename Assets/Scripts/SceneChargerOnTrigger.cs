using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneChangerOnTrigger : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a la que se cambiará

    // Método que se ejecuta cuando algo entra en el trigger del objeto
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra tiene el tag 'Player'
        if (other.CompareTag("Player"))
        {
            // Cambia a la escena especificada
            Debug.Log("Jugador detectado, cambiando a la escena: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }
}
