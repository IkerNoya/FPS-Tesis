using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeLauncher : Weapon
{
    [SerializeField] GameObject granade;
    [SerializeField] float granadeSpeed;

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
        if (Input.GetButtonDown("Fire1") && canShoot && shootTimer <= 0 && currentAmmo > 0)
        {
            Shoot();
            mouseLook.AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < ammo && !isReloading)
        {
            if (canReload)
                StartCoroutine(Reload(reloadSpeed));
        }
        shootTimer -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);

        GameObject go = Instantiate(this.granade, shootingPoint.position, Quaternion.identity);
        Granade granade = go.GetComponent<Granade>();
        if (granade != null)
        {
            granade.Initialize(shootingPoint, granadeSpeed, 9.8f);
            Destroy(go, 5);
        }

        currentAmmo--;
        shootTimer = fireRate;
    }
}
