using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField] GameObject granade;
    [SerializeField] ParticleSystem explotion;

    Rigidbody rb;
    Vector3 direction;
    Vector3 selectedDirection;
    float force;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        selectedDirection = direction;
        rb.AddForce(selectedDirection * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Environment"))
        {
            if(explotion!=null)
                explotion.Play();

            granade.SetActive(false);
            rb.isKinematic = true;
            rb.useGravity = false;

            Destroy(gameObject, 2);
        }
    }
    public void SetForce(float value)
    {
        force = value;
    }
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
}
