using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
        RaycastHit hit;
        Vector3 mousePos;
        Ray ray;
    void Start()
    {
        currentAmmo = ammo;
    }
    void Update()
    {
        mousePos = Input.mousePosition;
        ray = cam.ScreenPointToRay(mousePos);
        Debug.DrawRay(transform.position, ray.direction * range, Color.magenta);
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

    protected override void Shoot() {

        if (Physics.Raycast(ray, out hit, range)) {
            Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Enemy")) {
                Debug.Log("ASD");
                hit.collider.GetComponent<IHittable>().HitWithStun(damage, stunDuration);
            }
        }

        currentAmmo--;
        shootTimer = fireRate;
    }

}
