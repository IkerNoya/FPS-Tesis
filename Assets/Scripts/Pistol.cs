﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    void Start()
    {
        currentAmmo = ammo;
    }
    void Update()
    {
        if(Input.GetButton("Fire1") && canShoot && shootTimer <= 0 && currentAmmo > 0)
        {
            Shoot();
            mouseLook.AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < ammo && !isReloading)
        {
            StartCoroutine(Reload(reloadSpeed));
        }
        shootTimer -= Time.deltaTime;
    }
}
