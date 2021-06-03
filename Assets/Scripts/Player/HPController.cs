using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HPController : MonoBehaviour
{
    [SerializeField] int maxHP;

    float hp;

    bool isAlive=true;
    bool canHeal = false;
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

    void TakeDamage(Player player, int value)
    {
        hp -= value;
        canHeal = false;
    }

    private void OnDestroy()
    {
        Player.TakeDamage -= TakeDamage;
    }
}
