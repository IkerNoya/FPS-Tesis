﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    void Update()
    {
        if(Input.GetButton("Fire1") && canShoot && shootTimer <= 0 && currentAmmo > 0)
        {
            Shoot();
            mouseLook.AddRecoil(verticalRecoil, horizontalRecoil);
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < ammo && !isReloading)
        {
            StartCoroutine(Reload(reloadSpeed));
        }
        shootTimer -= Time.deltaTime;
    }
}
