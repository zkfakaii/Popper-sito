using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Daño que inflige el proyectil
    public float lifetime = 5f; // Tiempo de vida del proyectil

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destruye el proyectil después de un tiempo
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage((int)damage);
            Debug.Log("Proyectil golpea al jugador: " + other.name);
            Destroy(gameObject); // Destruye el proyectil al golpear al jugador
        }
        else
        {
            Debug.Log("Proyectil colisiona con: " + other.name);
            Destroy(gameObject); // Destruye el proyectil al colisionar con cualquier otra cosa
        }
    }
}
