using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    HPController hpController;
    int damageTaken = 20;
    public static event Action<Player, int> TakeDamage;
    void Start()
    {
        hpController = GetComponent<HPController>();
    }

    void Update()
    {
        if (!hpController.GetIsAlive())
            return;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            TakeDamage?.Invoke(this, damageTaken);
        }

        if (hpController.GetHP() <= ((hpController.GetMaxHP() * 20) / 100))
        {
            Debug.Log("valor: " + (hpController.GetMaxHP() * 20) / 100);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage?.Invoke(this, damageTaken);
        }
    }
}
