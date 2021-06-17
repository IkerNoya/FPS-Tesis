using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    HPController hpController;
    Player player;

    public Text healthPoints;
    public Text ammo;


    void Start()
    {
        hpController = GetComponent<HPController>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        if(healthPoints!=null)
            healthPoints.text = "HP: " + ((int)hpController.GetHP()).ToString();
        if (player != null)
            ammo.text = player.GetWeapons()[(int)player.GetWeaponMode()].GetCurrentAmmo().ToString() + " / " + player.GetWeapons()[(int)player.GetWeaponMode()].GetMaxAmmo().ToString();
    }

    public void OnClickWeapon(int weapon)
    {
        player.SetWeapon((Player.WeaponMode)weapon);
    }
}
