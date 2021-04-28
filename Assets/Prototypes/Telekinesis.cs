using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    [SerializeField] float targetMoveSpeed;
    [SerializeField] float pushForce;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Camera cam;
    [SerializeField] Transform finalPos;
    [SerializeField] Transform target;
    

    bool canPull = true;
    bool moveToPos = false;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Q) && canPull && Physics.Raycast(ray, out hit, 30, targetLayer))
        {
            target = hit.transform;
            moveToPos = true;
            target.GetComponent<Rigidbody>().useGravity = false;
            target.GetComponent<Rigidbody>().velocity = Vector3.zero;
            canPull = false;
        }
        if (moveToPos)
        {
            if (target != null) target.position = Vector3.Lerp(target.position, finalPos.position, Time.deltaTime * targetMoveSpeed);
        }
        else
        {
            if(target!=null) target.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && target!=null)
        {
            target.GetComponent<Rigidbody>().useGravity = true;
            target.GetComponent<Rigidbody>().AddForce(ray.direction * pushForce);
            moveToPos = false;
            target = null;
            canPull = true;
        }
    }

}
