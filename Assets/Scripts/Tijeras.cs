using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tijeras : MonoBehaviour
{
    public int damage = 1; // Daño que inflige la esfera

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si colisiona con un enemigo, inflige daño
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("Esfera ha colisionado con " + collision.gameObject.name + ". Infligiendo " + damage + " de daño.");
                enemyHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("El objeto colisionado no tiene un componente EnemyHealth.");
            }

            // Destruye la esfera después de colisionar
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Esfera colisionó con " + collision.gameObject.name + ", que no es un enemigo.");
        }
    }
}
