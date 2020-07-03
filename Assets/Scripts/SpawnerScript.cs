using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [System.Serializable]
    public class Wave                   // Classe Wave
    {
        public GameObject[] enemies;    // Lista de possiveis inimigos em uma wave
        public int enemyCount;          // Quantidade de inimigos em uma wave
        public float spawnTime;         // Tempo de spawn entre os inimigos em uma wave
    }

    public Wave[] waves;                // Lista de waves
    public Transform[] spawnPoints;     // Pontos de spawn dos inimigos
    public float waveTime;              // Tempo entre cada wave
    public GameScript gameScript;       // Referencia ao Game manager

    private Wave currentWave;           // Variavel auxiliar para guardar a wave atual
    private int currentWaveIndex;       // Variavel auxiliar para saber o numero da wave atual
    private Transform player;           // Referencia do player
    private bool waveEnded;             // Determina se a wave ja acabou, ou ainda falta inimigo para spawnar


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Busca a referencia do player
        StartCoroutine(StartNextWave(currentWaveIndex));                // Inicia o processo das waves
    }

    IEnumerator StartNextWave(int index)
    {
        yield return new WaitForSeconds(waveTime); // Aguarda o tempo entre cada waves
        StartCoroutine(SpawnWave(index));          // Apos passar esse tempo, pode iniciar o spawn
    }

    IEnumerator SpawnWave(int index)
    {
        // Carrega o numero da wave atual
        currentWave = waves[index];

        // Faz um loop, executando a funcao para cada inimigo
        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            if (player == null) yield break; // Se o player foi destruido, sai da funcao

            GameObject randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];  // Busca um inimigo aleatorio, entre os possiveis inimigos da wave
            Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];                    // Busca um ponto aleatorio, entre os spawn points
            Instantiate(randomEnemy, randomSpot.position, randomSpot.rotation);                         // Cria o inimigo, naquele ponto que acabou de buscar

            // Se ja spawnou todos os inimigos, marca que a wave acabou. Caso contrario (else), marca que ainda nao acabou
            if (i == currentWave.enemyCount - 1)
            {
                waveEnded = true;
            }
            else
            {
                waveEnded = false;
            }

            // Aguarda o tempo de spawn entre cada inimigo
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Verifica se terminou de spawnar a wave, e tambem se nao existe nenhum inimigo ativo
        if (waveEnded == true
        && GameObject.FindGameObjectsWithTag("Enemy").Length == 0
        && GameObject.FindGameObjectsWithTag("EnemyRanged").Length == 0
        && GameObject.FindGameObjectsWithTag("Boss").Length == 0)
        {
            waveEnded = false; // Marca que a wave nao acabou (iniciando a proxima wave)

            // Verifica se ainda tem alguma wave para spawnar
            if (currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++; // Aumenta o numero da wave atual
                StartCoroutine(StartNextWave(currentWaveIndex)); // Inicia proxima wave
            }
            else // Aqui nao existe mais wave para spawnar
            {
                // Chama a funcao de vitoria do Game manager
                gameScript.Victory();
            }
        }

    }
}
