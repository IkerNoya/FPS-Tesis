using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IHittable
{
    public enum WeaponMode
    {
        Pistol, SMG, Shotgun, Rifle, GranadeLauncher
    }

    [SerializeField] WeaponMode equipedWeapon;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<Weapon> weaponsScript;

    HPController hpController;

    public static event Action<Player> Died;
    public static event Action<Player> TakeDamage;

    int damageTaken = 20;

    float timer;
    float waitForHpRegen = 5f;
    bool canSwitchWeapons = true;

    void Start()
    {
        hpController = GetComponent<HPController>();
        for (int i = 0; i < weapons.Count; i++)
            weapons[i].SetActive(false);

        weapons[(int)equipedWeapon].SetActive(true);
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
        StartCoroutine(WeaponSwitch());
    }
    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && canSwitchWeapons)
        {
            equipedWeapon = WeaponMode.Pistol;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && canSwitchWeapons)
        {
            equipedWeapon = WeaponMode.SMG;
            SetWeapon();    
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && canSwitchWeapons)
        {
            equipedWeapon = WeaponMode.Shotgun;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && canSwitchWeapons)
        {
            equipedWeapon = WeaponMode.Rifle;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && canSwitchWeapons)
        {
            equipedWeapon = WeaponMode.GranadeLauncher;
            SetWeapon();
        }
    }

    public WeaponMode GetWeaponMode()
    {
        return equipedWeapon;
    }

    public List<Weapon> GetWeapons()
    {
        return weaponsScript;
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

    IEnumerator WeaponSwitch()
    {
        canSwitchWeapons = false;
        for (int i = 0; i < weapons.Count; i++)
        {
            if(i == (int)equipedWeapon)
            {
                weaponsScript[i].setCanShoot(false);
                weapons[i].GetComponent<Animator>().SetTrigger("WeaponChange");
                yield return new WaitForSeconds(1);
            }
            weapons[i].SetActive(false);
        }

        weaponsScript[(int)equipedWeapon].setCanShoot(false);
        weapons[(int)equipedWeapon].SetActive(true);
        weapons[(int)equipedWeapon].GetComponent<Animator>().SetTrigger("WeaponEquip");
        yield return new WaitForSeconds(1);
        weaponsScript[(int)equipedWeapon].setCanShoot(true);
        canSwitchWeapons = true;
        yield return null;
    }
}
