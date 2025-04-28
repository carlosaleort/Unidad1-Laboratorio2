using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public int count;
    }

    public List<EnemyType> waveEnemies = new List<EnemyType>();
    public float spawnRadius = 10f;
    public float spawnDelay = 0.2f; // Pequeño delay entre spawns
    public float timeBetweenWaves = 5f;

    private int currentWave = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;

        Debug.Log($"Starting Wave {currentWave + 1}");

        foreach (var enemyType in waveEnemies)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPosition.y = 0; // Asumiendo plano de suelo

                GameObject enemy = Instantiate(enemyType.prefab, spawnPosition, Quaternion.identity);

                // Asegurarse que el material del enemigo tiene GPU Instancing activado
                Renderer rend = enemy.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    foreach (var mat in rend.sharedMaterials)
                    {
                        if (mat != null && !mat.enableInstancing)
                            mat.enableInstancing = true;
                    }
                }

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        currentWave++;
        isSpawning = false;

        // Esperar antes de la siguiente oleada
        yield return new WaitForSeconds(timeBetweenWaves);

        // Lanzar próxima oleada
        StartCoroutine(SpawnWave());
    }
}
