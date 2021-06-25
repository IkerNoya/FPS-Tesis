using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemySpawner spawner;
    public static event Action AllEnemiesKilled;
    public static event Action<bool> AllHordesKilled;

    [SerializeField] List<Enemy> enemiesAlive;
    bool allEnemiesCreated = false;
    void Start()
    {
        EnemySpawner.EnemyCreated += CreatedEnemy;
        Enemy.Killed += KilledEnemy;

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
        if (enemiesAlive.Count <= 0 && allEnemiesCreated && !spawner.GetCanWin())
                AllEnemiesKilled?.Invoke();
        else if (enemiesAlive.Count <= 0 && allEnemiesCreated && spawner.GetCanWin())
            AllHordesKilled?.Invoke(true);
    }

}
