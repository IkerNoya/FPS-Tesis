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
            canPull = false;
        }
        if (moveToPos)
        {
            target.position = Vector3.Lerp(target.position, finalPos.position, Time.deltaTime * targetMoveSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && target!=null)
        {
            target.GetComponent<Rigidbody>().useGravity = true;
            target.GetComponent<Rigidbody>().AddForce(transform.forward * pushForce);
            moveToPos = false;
            target = null;
            canPull = true;
        }
    }
}
