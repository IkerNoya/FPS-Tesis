using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    HPController hpController;

    public static event Action<Player> Died;

    int damageTaken = 20;

    float timer;
    float waitForHpRegen = 5f;

    void Start()
    {
        hpController = GetComponent<HPController>();
    }

    void Update()
    {
        if (!hpController.GetIsAlive() && hpController!=null)
            return;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (hpController != null)
            {
                hpController.TakeDamage(damageTaken);
                hpController.SetCanHeal(false);
            }
            timer = 0;
        }

        if (hpController != null)
        {
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
            
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (hpController != null)
            {
                hpController.TakeDamage(damageTaken);
                hpController.SetCanHeal(false);
                if (hpController.GetHP() <= 0)
                    Died?.Invoke(this);
            }
            timer = 0;
        }
    }
}
