using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IHittable {
    public enum WeaponMode {
        Pistol, SMG, Shotgun, Rifle
    }
    [Header("Weapons")]
    [SerializeField] GameObject weaponWheel;
    [SerializeField] WeaponMode equipedWeapon;
    [SerializeField] List<Weapon> weapons;
    [Header("Controllers")]
    [SerializeField] MouseLook cameraMovement;
    [SerializeField] HPController hpController;
    [SerializeField] PlayerHUD hud;
    [Header("Granade")]
    [SerializeField] Transform handPosition;
    [SerializeField] GameObject granade;
    [SerializeField] float granadeSpeed;
    [SerializeField] float granadeTimerLimit;

    public static event Action<bool> Died;
    public static event Action<Player> TakeDamage;
    public static event Action PickedUpUpgrade;
    public static event Action<float> granadeThrow;

    int damageTaken = 20;
    int granadeInventory = 1;

    float timer = 0;
    float granadeTimer = 0;
    float waitForHpRegen = 5f;

    bool canSwitchWeapons = true;
    bool isWeaponWheelActivated = false;
    bool allWeaponsUnlocked = false;

    void Start() {

        for (int i = 0; i < weapons.Count; i++)
            weapons[i].gameObject.SetActive(false);

        weapons[(int)equipedWeapon].gameObject.SetActive(true);

        if (weaponWheel)
            weaponWheel.SetActive(false);

        Weapon.WeaponShot += AmmoChanged;
        PlayerHUD.ClickedWeapon += SetWeapon;
        ObjectiveManager.CompletedObjective += ObjectiveCompleted;

        hud.SetUpgradedWeapons(false);
        ChangedHP(hpController.GetHP());
        AmmoChanged(weapons[(int)equipedWeapon].GetCurrentAmmo(), weapons[(int)equipedWeapon].GetMaxAmmo());
        GranadeInventoryChanged();

        granadeTimer = granadeTimerLimit;
    }

    private void OnDisable() {
        Weapon.WeaponShot -= AmmoChanged;
        PlayerHUD.ClickedWeapon -= SetWeapon;
        ObjectiveManager.CompletedObjective -= ObjectiveCompleted;
    }

    void Update() {
        if (!hpController.GetIsAlive())
            return;


        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            Hit(damageTaken);
        }
        if(granadeTimer<granadeTimerLimit)
        {
            for (int i = 0; i < hud.GetGranadeBarObjects().Length; i++)
            {
                hud.GetGranadeBarObjects()[i].SetActive(true);
            }
            granadeThrow?.Invoke(granadeTimer / granadeTimerLimit);
            granadeTimer += Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < hud.GetGranadeBarObjects().Length; i++)
            {
                if(hud.GetGranadeBarObjects()[i].activeSelf)
                    hud.GetGranadeBarObjects()[i].SetActive(false);
            }
        }
        if (hpController != null) {
            if (!hpController.GetCanHeal()) {
                if (timer >= waitForHpRegen) {
                    hpController.SetCanHeal(true);
                    timer = 0;
                }
                timer += Time.deltaTime;
            }
            else {
                if (hpController.GetHP() < hpController.GetMaxHP()) {
                    hpController.RegenerateHP(10);
                }
            }
        }
        CheckReload();
        Inputs();
        ActivateWeaponWheel();
    }
    void Inputs() {
        if (allWeaponsUnlocked) {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetWeapon(WeaponMode.Pistol);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SetWeapon(WeaponMode.SMG);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SetWeapon(WeaponMode.Shotgun);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                SetWeapon(WeaponMode.Rifle);
        }
        if (Input.GetKeyDown(KeyCode.G) && granadeInventory > 0 && granadeTimer >= granadeTimerLimit)
        {
            granadeInventory--;
            GameObject go = Instantiate(this.granade, handPosition.position, Quaternion.identity);
            Granade granade = go.GetComponent<Granade>();
            if (granade != null)
            {
                granade.Initialize(handPosition, granadeSpeed, 9.8f);
                Destroy(go, 10);
            }
            GranadeInventoryChanged();
            granadeTimer = 0;
        }

        if (Input.GetKey(KeyCode.Tab)) {
            isWeaponWheelActivated = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab)) {
            isWeaponWheelActivated = false;
            Time.timeScale = 1;
            if (weaponWheel)
                weaponWheel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            if (canSwitchWeapons)
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
    void CheckReload()
    {
        if (weapons[(int)equipedWeapon].GetIsReloading())
        {
            for (int i = 0; i < hud.GetReloadBarObjects().Length; i++)
            {
                hud.GetReloadBarObjects()[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < hud.GetReloadBarObjects().Length; i++)
            {
                hud.GetReloadBarObjects()[i].SetActive(false);
            }
        }
    }
    void ActivateWeaponWheel() {
        if (weaponWheel == null || !isWeaponWheelActivated)
            return;

        Time.timeScale = 0.5f;
        weaponWheel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        weapons[(int)equipedWeapon].SetCanShoot(false);
        cameraMovement.SetCanMoveCamera(false);
    }
    IEnumerator ActivateHint(float time)
    {
        hud.GetHintText().SetActive(true);
        yield return new WaitForSeconds(time);
        hud.GetHintText().SetActive(false);
        yield return null;
    }
    void ObjectiveCompleted()
    {
        StartCoroutine(ActivateHint(5));
    }
    public void UnlockAllWeapons() {
        PickedUpUpgrade?.Invoke();
        allWeaponsUnlocked = true;
        granadeInventory += 5;
        GranadeInventoryChanged();
        hud.SetUpgradedWeapons(true);

    }
    public WeaponMode GetWeaponMode() {
        return equipedWeapon;
    }

    public List<Weapon> GetWeapons() {
        return weapons;
    }

    public void Hit(float damage) {
        if (hpController != null) {
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

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Hit(damageTaken);
        }
    }

    IEnumerator WeaponSwitch() {
        canSwitchWeapons = false;
        for (int i = 0; i < weapons.Count; i++) {
            if (i == (int)equipedWeapon) {
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

    public void SetWeapon(WeaponMode mode) {
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
    void GranadeInventoryChanged()
    {
        hud.ChangeGranadeText(granadeInventory);
    }
    public void HitWithStun(float damage, float stunDuration) {
        //NADA 
    }

}
