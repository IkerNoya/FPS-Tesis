using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float groundDistance;
    [SerializeField] float jumpHeight;
    [SerializeField] Camera cam;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    public bool crouchToggle;
    public bool sprintToggle;

    CharacterController controller;

    float gravity = -9.81f * 2;
    float yNegativeVelocity = -2;

    Vector3 movement;
    Vector3 velocity;

    bool isGrounded;
    bool isSprinting;
    bool isCrouched;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
            velocity.y = yNegativeVelocity;
        

        //movement + sprint
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = transform.right * x + transform.forward * z;

        //jump + artificial gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        Inputs();
        Movement();
    }
    void Inputs()
    {
        //movement
        if (!sprintToggle)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isSprinting = true;
                isCrouched = false;
            }
            else
                isSprinting = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting)
            {
                isSprinting = true;
                isCrouched = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift) && isSprinting)
                isSprinting = false;
        }

        if(!crouchToggle)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isCrouched = true;
                isSprinting = false;
            }
            else
                isCrouched = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouched)
            {
                isCrouched = true;
                isSprinting = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouched)
                isCrouched = false;
        }

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // jump formula: result = sqrt( h * -2 * g)
    }
    void Movement()
    {
        if(isSprinting)
            controller.Move(movement * sprintSpeed * Time.deltaTime);
        if (isCrouched)
            controller.Move(movement * crouchSpeed * Time.deltaTime);
        if (!isSprinting && !isCrouched)
            controller.Move(movement * speed * Time.deltaTime);
    }
}
