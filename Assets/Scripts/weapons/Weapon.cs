using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{

    [SerializeField] protected Camera cam;
    [SerializeField] protected MouseLook mouseLook;

    [SerializeField] protected int ammo;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float damage;
   
    [SerializeField] protected float verticalRecoil;
    [SerializeField] protected float horizontalRecoil;
    
    [SerializeField] protected float range;
    [SerializeField] protected float reloadSpeed;
   
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject hitParticle;
    [SerializeField] protected ParticleSystem enemyHitParticle;

    [SerializeField] protected Transform shootingPoint; 
    [SerializeField] protected float stunDuration;

    protected float shootTimer = 0;

    protected int currentAmmo;

    protected bool isReloading = false;
    protected bool canShoot = true;

    protected bool canReload = true;

    protected RaycastHit hit;
    protected Vector3 mousePos;
    protected Ray ray;

    public static event Action<int, int> WeaponShooted;

    protected virtual void Start() {
        currentAmmo = ammo;
    }

    protected virtual void Update() {
        mousePos = Input.mousePosition;
        ray = cam.ScreenPointToRay(mousePos);
        Debug.DrawRay(transform.position, ray.direction * range, Color.magenta);
        shootTimer -= Time.deltaTime;
    }
    public virtual void Shoot()
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce((hit.point - transform.position) * 30);
            }
        }

        currentAmmo--;
        shootTimer = fireRate;

        AmmoChanged();
    }
    protected void AmmoChanged() {
        if (WeaponShooted != null)
            WeaponShooted?.Invoke(currentAmmo, ammo);
    }
    public virtual void Reload() {
        if (currentAmmo < ammo && !isReloading && canReload)
            StartCoroutine(Reload(reloadSpeed));
    }
    protected IEnumerator Reload(float timer)
    {
        isReloading = true;
        canShoot = false;
        yield return new WaitForSeconds(timer);
        currentAmmo = ammo;
        canShoot = true;
        isReloading = false;

        if (WeaponShooted != null)
            WeaponShooted?.Invoke(currentAmmo, ammo);

        yield return null;
    }
   public void StopWeapon()
    {
        canReload = false;
        canShoot = false;
    }
    public int GetMaxAmmo()
    {
        return ammo;
    }
    
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    public void CanShootAfterSpawn()
    {
        canShoot = true;
    }
    public void SetCanShoot(bool value)
    {
        canShoot = value;
    }
    public bool GetCanShoot()
    {
        return canShoot;
    }
}
