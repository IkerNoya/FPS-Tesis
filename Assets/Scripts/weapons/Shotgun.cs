using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Pellets")]
    [SerializeField] int pellets;

    Vector2[] points;

    protected override void Start()
    {
        base.Start();

    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Reload()
    {
        base.Reload();
    }
    public override void Shoot()
    {
        if (!canShoot || shootTimer > 0 || currentAmmo <= 0)
            return;

        points = new Vector2[pellets];
        for (int i = 0; i < points.Length; i++)
            points[i] = new Vector2(Next(0, 1, -1, 1), Next(0, 1, -1, 1));

        mouseLook.AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
        for(int i = 0; i < pellets; i++)
        {
            Vector3 spread = Vector3.zero;
            spread += new Vector3(ray.direction.x + points[i].x, ray.direction.y + points[i].y, ray.direction.z + points[i].x); 
            if (Physics.Raycast(ray.origin, ray.direction + spread.normalized * Random.Range(0f,0.1f) , out hit, range))
            {
                Debug.DrawRay(ray.origin, (ray.direction + spread.normalized * Random.Range(0f, 0.1f)) * range, Color.red);
                if (hit.collider.CompareTag("Enemy"))
                {
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
                    GameObject go = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.transform.forward, hit.normal));
                    Debug.DrawRay(hit.point, hit.normal * 100, Color.red);
                    Destroy(go.gameObject, 1.5f);
                }
            }

        }

        currentAmmo--;
        shootTimer = fireRate;

        AmmoChanged();
    }


    // George Marsaglia's polar algorithm for random gaussian distribution over area
    
    float _spareResult;
    bool _nextResultReady = false;
    public float Next()
    {
        float result;

        if (_nextResultReady)
        {
            result = _spareResult;
            _nextResultReady = false;

        }
        else
        {
            float s = -1f, x, y;

            do
            {
                x = 2f * Random.value - 1f;
                y = 2f * Random.value - 1f;
                s = x * x + y * y;
            } while (s < 0f || s >= 1f);

            s = Mathf.Sqrt((-2f * Mathf.Log(s)) / s);

            _spareResult = y * s;
            _nextResultReady = true;

            result = x * s;
        }

        return result;
    }

    public float Next(float mean, float sigma = 1f) => mean + sigma * Next();

    public float Next(float mean, float sigma, float min, float max)
    {
        float x = min - 1f; while (x < min || x > max) x = Next(mean, sigma);
        return x;
    }
}
