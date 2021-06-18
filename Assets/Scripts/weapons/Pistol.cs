using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{

   protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();        
    }
    public override void Reload() {
        base.Reload();
    }

    public override void Shoot() {
        if (!canShoot || shootTimer > 0 || currentAmmo <= 0)
            return;

         mouseLook.AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
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

        AmmoChanged();
    }

}
