using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected float shootTimer = 0;

    protected int currentAmmo;

    protected bool isReloading = false;
    protected bool canShoot = true;

    [SerializeField] protected float stunDuration;
    protected bool canReload = true;
    void Awake()
    {
        Player.Died += PlayerDied;
    }
    protected virtual void Shoot()
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
    }
    protected IEnumerator Reload(float timer)
    {
        isReloading = true;
        canShoot = false;
        yield return new WaitForSeconds(timer);
        currentAmmo = ammo;
        canShoot = true;
        isReloading = false;
        yield return null;
    }
    void PlayerDied(Player player)
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
    void OnDestroy()
    {
        Player.Died -= PlayerDied;
    }
}
