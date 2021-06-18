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

    [SerializeField] GameObject weaponWheel;
    [SerializeField] WeaponMode equipedWeapon;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<Weapon> weaponsScript;
    [SerializeField] MouseLook cameraMovement;
    HPController hpController;

    public static event Action<Player> Died;
    public static event Action<Player> TakeDamage;

    int damageTaken = 20;

    float timer;
    float waitForHpRegen = 5f;

    bool canSwitchWeapons = true;
    bool isWeaponWheelActivated = false;

    void Start()
    {
        hpController = GetComponent<HPController>();
        for (int i = 0; i < weapons.Count; i++)
            weapons[i].SetActive(false);

        weapons[(int)equipedWeapon].SetActive(true);

        if(weaponWheel)
            weaponWheel.SetActive(false);

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
        ActivateWeaponWheel();
    }
    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(WeaponMode.Pistol);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(WeaponMode.SMG);    
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeapon(WeaponMode.Shotgun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetWeapon(WeaponMode.Rifle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetWeapon(WeaponMode.GranadeLauncher);
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            isWeaponWheelActivated = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isWeaponWheelActivated = false;
            Time.timeScale = 1;
            if (weaponWheel)
                weaponWheel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            if(canSwitchWeapons)
                weaponsScript[(int)equipedWeapon].setCanShoot(true);

            cameraMovement.SetCanMoveCamera(true);
        }
    }

    void ActivateWeaponWheel()
    {
        if (weaponWheel == null || !isWeaponWheelActivated)
            return;

        Time.timeScale = 0.5f;
        weaponWheel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        weaponsScript[(int)equipedWeapon].setCanShoot(false);
        cameraMovement.SetCanMoveCamera(false);
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

    public void SetWeapon(WeaponMode mode)
    {
        if (!canSwitchWeapons)
            return;

        equipedWeapon = mode;
        StartCoroutine(WeaponSwitch());
    }
    public void HitWithStun(float damage, float stunDuration)
    {
        //NADA 
    }

}
