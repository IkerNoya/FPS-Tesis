using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemySpawner spawner;
    public static event Action<bool> AllEnemiesKilled;

    [SerializeField] List<Enemy> enemiesAlive;
    bool allEnemiesCreated = false;
    void Start()
    {
        EnemySpawner.EnemyCreated += CreatedEnemy;
        Enemy.Killed += KilledEnemy;

        spawner.SetCanCreateEnemies(true);
    }
    void OnDisable() {
        EnemySpawner.EnemyCreated -= CreatedEnemy;
        Enemy.Killed -= KilledEnemy;
    }

    void CreatedEnemy(Enemy e) {
        enemiesAlive.Add(e);
        allEnemiesCreated = spawner.GetAllEnemiesCreated();
    }
    void KilledEnemy(Enemy e) {
        enemiesAlive.Remove(e);
        if (enemiesAlive.Count <= 0 && allEnemiesCreated)
            if (AllEnemiesKilled != null)
                AllEnemiesKilled(true);
    }

}
