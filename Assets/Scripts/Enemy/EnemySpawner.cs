using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<Transform> spawnerPositions;
    [SerializeField] List<Enemy> enemiesPrefabs;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float timerToSpawn;
    [SerializeField] int enemiesCreated;
    [SerializeField] int maxCantOfEnemiesToSpawn;
    [SerializeField] int enemiesToCreateForIteration;
    [SerializeField] Transform enemiesParent;

    public static event Action<EnemySpawner> EndGame;

    void Update() {
        if (enemiesCreated >= maxCantOfEnemiesToSpawn){
            EndGame?.Invoke(this);
            return;
        }

        timerToSpawn += Time.deltaTime;
        if(timerToSpawn >= timeBetweenSpawns) {
            timerToSpawn = 0;

            List<Transform> spawnerPositionsAux = new List<Transform>(spawnerPositions);
            for (int i = 0; i < enemiesToCreateForIteration; i++) {
                if (enemiesCreated >= maxCantOfEnemiesToSpawn)
                    break;

                int randomPosition = UnityEngine.Random.Range(0, spawnerPositionsAux.Count);
                Enemy e = Instantiate(enemiesPrefabs[UnityEngine.Random.Range(0, enemiesPrefabs.Count)], spawnerPositionsAux[randomPosition].position, Quaternion.identity, enemiesParent);

                spawnerPositionsAux.RemoveAt(randomPosition);
                enemiesCreated++;
            }
        }
    }
}
