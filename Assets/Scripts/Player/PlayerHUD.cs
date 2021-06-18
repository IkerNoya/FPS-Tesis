using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Text hpText;
    [SerializeField] Text ammoText;

    public static event Action<int> ClickedWeapon;

    public void ChangeHPText(float hp) {
        hpText.text = "HP: " + hp;
    }
    public void ChangeAmmoText(int actualAmmo, int totalAmmo) {
        ammoText.text = actualAmmo + " / " + totalAmmo;
    }

    public void OnClickWeapon(int weapon)
    {
        if (ClickedWeapon != null)
            ClickedWeapon(weapon);
    }
}
