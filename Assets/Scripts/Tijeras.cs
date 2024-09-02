using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tijeras : MonoBehaviour
{
    public int damage = 1; // Da�o que inflige la esfera

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si colisiona con un enemigo, inflige da�o
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("Esfera ha colisionado con " + collision.gameObject.name + ". Infligiendo " + damage + " de da�o.");
                enemyHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("El objeto colisionado no tiene un componente EnemyHealth.");
            }

            // Destruye la esfera despu�s de colisionar
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Esfera colision� con " + collision.gameObject.name + ", que no es un enemigo.");
        }
    }
}
