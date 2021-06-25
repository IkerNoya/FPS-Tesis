using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Text hpText;
    [SerializeField] Text ammoText;

    [SerializeField] List<GameObject> upgradedWeapons;

    [SerializeField] GameObject pickupText;

    public static event Action<int> ClickedWeapon;

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
        hpText.text = "HP: " + hp;
    }
    public void ChangeAmmoText(int actualAmmo, int totalAmmo) {
        ammoText.text = actualAmmo + " / " + totalAmmo;
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
}
