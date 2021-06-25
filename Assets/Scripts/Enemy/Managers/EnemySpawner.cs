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
    bool canWin = false;

    public static event Action<Enemy> EnemyCreated;
    void Start()
    {
        Player.PickedUpUpgrade += RestartSpawning;    
    }
    void OnDisable()
    {
        Player.PickedUpUpgrade -= RestartSpawning;
    }

    void Update() {

        if (enemiesCreated >= maxCantOfEnemiesToSpawn)
            return;
        

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

                if (EnemyCreated != null)
                    EnemyCreated(e);
            }
        }
        Debug.Log("canWin: " + canWin);
    }
    public bool GetAllEnemiesCreated() {
        return (enemiesCreated >= maxCantOfEnemiesToSpawn);
    }

    void RestartSpawning()
    {
        canWin = true;
        enemiesCreated = 0;
    }
    public bool GetCanWin()
    {
        return canWin;  
    }
}
