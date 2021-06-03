using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    HPController hpController;
    int damageTaken = 20;
    public static event Action<Player, int> TakeDamage;
    float timer;
    float waitForHpRegen = 5f;
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
            hpController.SetCanHeal(false);
            timer = 0;
        }

        if (!hpController.GetCanHeal())
        {
            if (timer >= waitForHpRegen)
            {
                hpController.SetCanHeal(true);
                timer = 0;
            }
            timer += Time.deltaTime;
        }
        else 
        {
            if (hpController.GetHP() < hpController.GetMaxHP())
            {
                hpController.RegenerateHP(10);
            }
        }
            
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage?.Invoke(this, damageTaken);
            hpController.SetCanHeal(false);
            timer = 0;
        }
    }
}
