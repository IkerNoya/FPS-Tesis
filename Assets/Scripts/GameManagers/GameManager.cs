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
        Player.Died += EndGame;
        EnemySpawner.EndGame += EndGame;
    }

    void EndGame(Player p) {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("LoseScreen");
    }
    void EndGame(EnemySpawner es)
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("VictoryScreen");
    }

    void OnDisable()
    {
        EnemySpawner.EndGame -= EndGame;
        Player.Died -= EndGame;
    }
}
