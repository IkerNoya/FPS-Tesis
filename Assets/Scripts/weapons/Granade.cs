using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float damageRadius;
    [SerializeField] float mass;
    [SerializeField] float Velocity;
    [SerializeField] float gravity;
    [SerializeField] float lifeTime;
    [Space]
    [SerializeField] GameObject granade;
    [SerializeField] ParticleSystem explotion;

    Rigidbody rb;

    Vector3 startPosition;
    Vector3 forwardVector;
    bool isInitialized;
    float startTime = -1;
    float currentVelocity = 0;
    float lastVelocity = 0;

    bool hitTarget = false;
    bool exploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Transform startPoint, float speed, float gravity)
    {
        startPosition = startPoint.position;
        forwardVector = startPoint.forward.normalized;
        Velocity = speed;
        this.gravity = gravity;
        isInitialized = true;
        currentVelocity = Velocity;
    }

    void Update()
    {
        if (!isInitialized || startTime < 0)
            return;
        float currentTime = Time.time - startTime;
        Vector3 currentPoint = FindPointInParabola(currentTime);
        if(!hitTarget)
            transform.position = currentPoint;

        Debug.Log(exploded);
    }

    void FixedUpdate()
    {
        if (!isInitialized)
            return;
        if (startTime < 0) startTime = Time.time;

        RaycastHit hit;
        float currentTime = Time.time - startTime;
        float prevTime = currentTime - Time.deltaTime;
        float nextTime = currentTime + Time.deltaTime;
        Vector3 currentPoint = FindPointInParabola(currentTime);
        Vector3 nextPoint = FindNextPointInParabola(nextTime);

        if (prevTime > 0)
        {
            Vector3 prevPoint = FindPreviousPointInParabola(prevTime);
            if (ConstRayBetweenPoints(prevPoint, currentPoint, out hit))
            {
                OnHit(hit);
            }
        }

        if (ConstRayBetweenPoints(currentPoint, nextPoint, out hit))
        {
            OnHit(hit);
        }

    }

    bool ConstRayBetweenPoints(Vector3 startPoint, Vector3 endPoint, out RaycastHit hit)
    {
        return Physics.Raycast(startPoint, endPoint - startPoint, out hit, (endPoint - startPoint).magnitude);
    }

    Vector3 FindPointInParabola(float time)
    {
        Vector3 point = startPosition + (forwardVector * currentVelocity * time);
        Vector3 gravityVec = Vector3.down * gravity * time * time;
        return point + gravityVec;
    }
    Vector3 FindNextPointInParabola(float time)
    {
        Vector3 point = startPosition + (forwardVector * currentVelocity * time);
        Vector3 gravityVec = Vector3.down * gravity * time * time;
        return point + gravityVec;
    }

    Vector3 FindPreviousPointInParabola(float time)
    {
        Vector3 point = startPosition + (forwardVector * lastVelocity * time);
        Vector3 gravityVec = Vector3.down * gravity * time * time;
        return point + gravityVec;
    }

    void OnHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Environment") || hit.collider.CompareTag("Pickable"))
        {
            hitTarget = true;
            if (explotion != null && rb != null && granade != null)
            {
                granade.SetActive(false);

                rb.isKinematic = true;
                rb.useGravity = false;

                explotion.Play();
                Destroy(gameObject, 2);
            }
            if (!exploded)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
                foreach(Collider c in colliders)
                {
                    if (c.GetComponent<Enemy>() != null)
                    {
                        c.GetComponent<IHittable>().Hit(damage);
                    }

                    if (c.CompareTag("Pickable"))
                    {
                        c.attachedRigidbody.AddExplosionForce(1000, transform.position, damageRadius);
                    }
                }
                exploded = true;
            }
           
        }
    }

    public float GetGravity()
    {
        return gravity;
    }

    public float GetLifeTime()
    {
        return lifeTime;
    }
}
