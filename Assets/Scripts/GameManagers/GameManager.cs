using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    float timer = 0;
    float endGameTimer = 1.5f;

    void Start()
    {
        EnemySpawner.EndGame += EndGame;
    }

    void Update()
    {
        
    }

    void EndGame(EnemySpawner es)
    {
    
    }

    void OnDisable()
    {
        EnemySpawner.EndGame -= EndGame;
    }
}
