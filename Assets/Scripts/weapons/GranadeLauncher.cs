using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeLauncher : Weapon
{
    [SerializeField] GameObject granade;
    [SerializeField] float granadeSpeed;

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

    public override void Shoot()
    {
        if (!canShoot || shootTimer > 0 || currentAmmo <= 0)
            return;

        mouseLook.AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
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

        AmmoChanged();
    }
}
