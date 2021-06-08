using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IHittable
{
    enum WeaponMode
    {
        Pistol, SMG, Shotgun, Rifle, GranadeLauncher
    }

    [SerializeField] WeaponMode equipedWeapon;
    [SerializeField] List<GameObject> weapons;

    HPController hpController;

    public static event Action<Player> Died;
    public static event Action<Player> TakeDamage;

    int damageTaken = 20;

    float timer;
    float waitForHpRegen = 5f;
    

    void Start()
    {
        hpController = GetComponent<HPController>();
        SetWeapon();
    }

    void Update()
    {
        
        if (!hpController.GetIsAlive() && hpController != null)
            return;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Hit(damageTaken);
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

        Inputs();
    }

    void SetWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
            weapons[i].SetActive(false);

        weapons[(int)equipedWeapon].SetActive(true);
    }
    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            equipedWeapon = WeaponMode.Pistol;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            equipedWeapon = WeaponMode.SMG;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            equipedWeapon = WeaponMode.Shotgun;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            equipedWeapon = WeaponMode.Rifle;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            equipedWeapon = WeaponMode.GranadeLauncher;
            SetWeapon();
        }
    }

    public void Hit(float damage)
    {
        if (hpController != null)
        {
            hpController.TakeDamage((int)damage);
            hpController.SetCanHeal(false);
            if (hpController.GetHP() <= 0)
                Died?.Invoke(this);
            TakeDamage?.Invoke(this);
        }
        timer = 0;
    }
    public void HitWithStun(float damage, float stunDuration)
    {
        //NADA 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Hit(damageTaken);
        }
    }
}
