using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HPController : MonoBehaviour
{
    [SerializeField] int maxHP;


    float hp;

    bool isAlive=true;
    bool canHeal = false;
    void Start()
    {
        hp = maxHP;
    }

    void Update()
    {
        if (hp >= 0)
            return;

        isAlive = false;
        hp = 0;
    }

    public void RegenerateHP(int regenAmmount)
    {
        if (hp < maxHP)
            hp += regenAmmount * Time.deltaTime;

        if (hp > maxHP)
            hp = maxHP;
    }

    public float GetHP()
    {
        return hp;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }
    public bool GetCanHeal()
    {
        return canHeal;
    }
    public void SetCanHeal(bool value)
    {
        canHeal = value;
    }

    public void TakeDamage(int value)
    {
        hp -= value;
        canHeal = false;
    }

}
