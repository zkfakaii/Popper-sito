using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puño : MonoBehaviour
{
    public float damage = 20f; // Daño que inflige el ataque
    public float lifetime = 2f; // Tiempo de vida del ataque

    private void Start()
    {
        // Destruye el ataque después de un cierto tiempo
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Inflige daño al jugador
            other.GetComponent<PlayerHealth>().TakeDamage((int)damage);
            Debug.Log("Ataque colisiona con el jugador: " + other.name); // Mensaje de depuración
            Destroy(gameObject); // Destruye el prefab de ataque después de hacer daño
        }
        else
        {
            Debug.Log("Ataque colisiona con: " + other.name); // Mensaje de depuración para otros objetos
        }
    }
}
