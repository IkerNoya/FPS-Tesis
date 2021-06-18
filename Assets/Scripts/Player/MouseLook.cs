using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float verticalSensitivity;
    [SerializeField] float horizontalSensitivity;
    [SerializeField] Transform body;

    HPController hpController;

    float xRotation = 0;
    float verticalRecoil = 0;
    float horizontalRecoil = 0;
    bool canMoveCamera = true;

    void Start()
    {
        hpController = GetComponentInParent<HPController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if ((!hpController.GetIsAlive() && hpController != null) || !canMoveCamera)
            return;

        float mouseX = horizontalRecoil + Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;
        float mouseY = verticalRecoil + Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime;
        //restart recoil after 1 frame
        verticalRecoil = 0;
        horizontalRecoil = 0;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        if (body != null)
            body.Rotate(Vector3.up * mouseX);
    }
    public void AddRecoil(float vertical, float horizontal)
    {
        verticalRecoil += vertical;
        horizontalRecoil += horizontal;
    }
    public void SetCanMoveCamera(bool value)
    {
        canMoveCamera = value;
    }
}
