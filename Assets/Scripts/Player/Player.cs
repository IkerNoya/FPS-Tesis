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
    [SerializeField] List<Weapon> weapons;
    [SerializeField] MouseLook cameraMovement;
    [SerializeField] HPController hpController;
    [SerializeField] PlayerHUD hud;

    public static event Action<bool> Died;
    public static event Action<Player> TakeDamage;

    int damageTaken = 20;

    float timer;
    float waitForHpRegen = 5f;

    bool canSwitchWeapons = true;
    bool isWeaponWheelActivated = false;
    bool allWeaponsUnlocked = false;

    void Start()
    {

        for (int i = 0; i < weapons.Count; i++)
            weapons[i].gameObject.SetActive(false);

        weapons[(int)equipedWeapon].gameObject.SetActive(true);

        if(weaponWheel)
            weaponWheel.SetActive(false);

        Weapon.WeaponShooted += AmmoChanged;
        PlayerHUD.ClickedWeapon += SetWeapon;

        hud.SetUpgradedWeapons(false);
        ChangedHP(hpController.GetHP());
        AmmoChanged(weapons[(int)equipedWeapon].GetCurrentAmmo(), weapons[(int)equipedWeapon].GetMaxAmmo());
    }

    private void OnDisable() {
        Weapon.WeaponShooted -= AmmoChanged;
        PlayerHUD.ClickedWeapon -= SetWeapon;
    }

    void Update()
    {
        if (!hpController.GetIsAlive())
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
                weapons[(int)equipedWeapon].SetCanShoot(true);

            cameraMovement.SetCanMoveCamera(true);
        }

        if (Input.GetButton("Fire1")) 
            weapons[(int)equipedWeapon].Shoot();
        if (Input.GetKeyDown(KeyCode.R))
            weapons[(int)equipedWeapon].Reload();


        if (Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, 3))
                if (hit.collider.CompareTag("Pickable"))
                    hit.collider.GetComponent<PickUp>().PickUpUpgrade(this);
        }
    }

    void ActivateWeaponWheel()
    {
        if (weaponWheel == null || !isWeaponWheelActivated)
            return;

        Time.timeScale = 0.5f;
        weaponWheel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        weapons[(int)equipedWeapon].SetCanShoot(false);
        cameraMovement.SetCanMoveCamera(false);
    }
    public void UnlockAllWeapons() {
        allWeaponsUnlocked = true;
        hud.SetUpgradedWeapons(true);
    }
    public WeaponMode GetWeaponMode()
    {
        return equipedWeapon;
    }

    public List<Weapon> GetWeapons()
    {
        return weapons;
    }

    public void Hit(float damage)
    {
        if (hpController != null)
        {
            hpController.TakeDamage((int)damage);
            hpController.SetCanHeal(false);
            if (hpController.GetHP() <= 0) {
                for (int i = 0; i < weapons.Count; i++)
                    weapons[i].StopWeapon();
                Died?.Invoke(false);
            }
            TakeDamage?.Invoke(this);
            ChangedHP(hpController.GetHP());
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
                weapons[i].SetCanShoot(false);
                weapons[i].GetComponent<Animator>().SetTrigger("WeaponChange");
                yield return new WaitForSeconds(1);
            }
            weapons[i].gameObject.SetActive(false);
        }

        weapons[(int)equipedWeapon].SetCanShoot(false);
        weapons[(int)equipedWeapon].gameObject.SetActive(true);
        weapons[(int)equipedWeapon].GetComponent<Animator>().SetTrigger("WeaponEquip");
        yield return new WaitForSeconds(1);
        weapons[(int)equipedWeapon].SetCanShoot(true);
        canSwitchWeapons = true;
        yield return null;
    }
    
    public void SetWeapon(WeaponMode mode)
    {
        if (!canSwitchWeapons)
            return;

        equipedWeapon = mode;
        AmmoChanged(weapons[(int)equipedWeapon].GetCurrentAmmo(), weapons[(int)equipedWeapon].GetMaxAmmo());
        StartCoroutine(WeaponSwitch());
    }

    void SetWeapon(int mode) {
        SetWeapon((WeaponMode)mode);
    }
    void AmmoChanged(int actualAmmo, int maxAmmo) {
        hud.ChangeAmmoText(actualAmmo, maxAmmo);
    }
    void ChangedHP(float actualHP) {
        hud.ChangeHPText(actualHP);
    }
    public void HitWithStun(float damage, float stunDuration)
    {
        //NADA 
    }

}
