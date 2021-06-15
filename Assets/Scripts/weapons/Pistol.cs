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
            if(canReload)
                StartCoroutine(Reload(reloadSpeed));
        }
        shootTimer -= Time.deltaTime;
    }

    protected override void Shoot() {

        if (Physics.Raycast(ray, out hit, range)) {
            if (hit.collider.CompareTag("Enemy")) {
                if (hit.collider.GetComponent<IHittable>() != null) 
                {
                    hit.collider.GetComponent<IHittable>().HitWithStun(damage, stunDuration);
                }
                else
                {
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce((hit.point - transform.position) * 30);
                }
            }
            else
            {
                GameObject go = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.transform.forward,hit.normal));
                Debug.DrawRay(hit.point, hit.normal * 100, Color.red);
                Destroy(go.gameObject, 1.5f);
            }
        }

        currentAmmo--;
        shootTimer = fireRate;
    }

}
