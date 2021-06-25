using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float endGameTimer = 2.0f;

    void Start()
    {
        Player.Died += EndGame;
        //EnemyManager.AllEnemiesKilled += EndGame;
    }

    void EndGame(bool winGame) {
        if (winGame)
            StartCoroutine(ChangeScene("VictoryScreen"));
        else
            StartCoroutine(ChangeScene("LoseScreen"));
    }
    
    IEnumerator ChangeScene(string scene) {
        yield return new WaitForSeconds(endGameTimer);

        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(scene);
        yield return null;
    }

    void OnDisable()
    {
        //EnemyManager.AllEnemiesKilled -= EndGame;
        Player.Died -= EndGame;
    }
}
