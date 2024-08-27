using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo que deseas spawnear
    public int spawnAmount = 5;    // Cantidad de enemigos a spawnear
    public float spawnInterval = 2f; // Intervalo de tiempo entre spawns
    public Transform[] spawnPoints; // Array de puntos donde los enemigos pueden spawnear

    private int enemiesSpawned = 0; // Contador de enemigos spawneados

    private void Start()
    {
        // Comenzamos el proceso de spawn
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < spawnAmount)
        {
            // Esperamos el intervalo definido
            yield return new WaitForSeconds(spawnInterval);

            // Elegimos un punto aleatorio de spawn
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // Instanciamos el enemigo en el punto de spawn
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            enemiesSpawned++;
        }
    }
}
