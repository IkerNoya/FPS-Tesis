using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectiveManager : MonoBehaviour {

    [SerializeField] GameObject door;

    public static event Action CompletedObjective;
    void Start() {
        EnemyManager.AllEnemiesKilled += CompleteObjective;
    }

    private void OnDisable() {
        EnemyManager.AllEnemiesKilled -= CompleteObjective;
    }

    void CompleteObjective() {
        door.GetComponent<Animation>().Play();
        CompletedObjective?.Invoke();
    }

}
