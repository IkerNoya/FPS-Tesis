using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Text hpText;
    [SerializeField] Text ammoText;
    [SerializeField] Text granadeText;

    [SerializeField] GameObject[] reloadBarObjects;
    [SerializeField] GameObject[] granadeBarObjects;

    [SerializeField] Image reloadBar;
    [SerializeField] Image granadeBar;

    [SerializeField] List<GameObject> upgradedWeapons;

    [SerializeField] GameObject pickupText;
    [SerializeField] GameObject HintText;


    public static event Action<int> ClickedWeapon;
    void Start()
    {
        Weapon.WeaponReload += ReloadImage;
        Player.granadeThrow += GranadeBarImage;
        HintText.SetActive(false);
    }
    private void OnDisable()
    {
        Weapon.WeaponReload -= ReloadImage;
        Player.granadeThrow -= GranadeBarImage;
    }

    private void Update() {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 3)) {
            if (hit.collider.CompareTag("Pickable"))
                pickupText.SetActive(true);
            else
                pickupText.SetActive(false);
        }
        else
            pickupText.SetActive(false);
    }

    public void ChangeHPText(float hp) {
        hpText.text = "HP: " + (int)hp;
    }
    public void ChangeAmmoText(int actualAmmo, int totalAmmo)
    {
        ammoText.text = actualAmmo + " / " + totalAmmo;
    }
    public void ChangeGranadeText(int granadeAmmount)
    {
        granadeText.text = granadeAmmount.ToString();
    }
    public void SetUpgradedWeapons(bool value) {
        for (int i = 0; i < upgradedWeapons.Count; i++)
            upgradedWeapons[i].SetActive(value);
    }

    public void OnClickWeapon(int weapon)
    {
        if (ClickedWeapon != null)
            ClickedWeapon(weapon);
    }
    public GameObject[] GetReloadBarObjects()
    {
        return reloadBarObjects;
    }
    public GameObject[] GetGranadeBarObjects()
    {
        return granadeBarObjects;
    }
    public void ReloadImage(float value) 
    {
        reloadBar.fillAmount = value;
    }
    public void GranadeBarImage(float value)
    {
        granadeBar.fillAmount = value;
    }
    public GameObject GetHintText()
    {
        return HintText;
    }
}
