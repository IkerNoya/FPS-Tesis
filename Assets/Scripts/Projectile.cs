using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] public LayerMask ignoreLayer;
    [SerializeField] public float damage;
    [SerializeField] public Rigidbody rb;
   
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer != ignoreLayer) {
            if (other.GetComponent<IHittable>() != null)
                other.GetComponent<IHittable>().Hit(damage);

                Destroy(this.gameObject);
        }
    }

}
