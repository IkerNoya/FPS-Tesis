using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HPController : MonoBehaviour
{
    [SerializeField] int maxHP;

    int hp;

    bool isAlive=true;
    void Awake()
    {
        Player.TakeDamage += TakeDamage;
        hp = maxHP;
    }

    void Update()
    {
        if (hp >= 0)
            return;

        isAlive = false;
    }

    public void RegenerateHP(int regenAmmount)
    {
        if (hp < maxHP)
            hp += regenAmmount;

        if (hp > maxHP)
            hp = maxHP;
    }

    public int GetHP()
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

    void TakeDamage(Player player, int value)
    {
        hp -= value;
    }

    private void OnDestroy()
    {
        Player.TakeDamage -= TakeDamage;
    }
}
