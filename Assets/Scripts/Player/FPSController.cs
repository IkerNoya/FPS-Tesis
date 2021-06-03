using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchMovementSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float groundDistance;
    [SerializeField] float jumpHeight;
    [Space]
    [SerializeField] float headBobbingIntensity;
    [SerializeField] float walkingHeadBobFrequency;
    [SerializeField] float sprintingHeadBobFrequency;
    [SerializeField] float crouchingHeadVerticalAmplitude;
    [SerializeField] float standingHeadBobVerticalAmplitude;
    [Space]
    [SerializeField] Transform cam;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform Head;
    [SerializeField] Transform CrouchedHead;
    [SerializeField] LayerMask groundMask;

    CharacterController controller;
    HPController hpController;

    float gravity = -9.81f * 2;
    float yNegativeVelocity = -2;
    float timeWalking;
    float hBobFrequency;
    float hBobVerticalAmplitude;
    float axisDifference = 0.001f;

    Vector3 movement;
    Vector3 velocity;

    bool isGrounded;
    bool isSprinting;
    bool isCrouched;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hpController = GetComponent<HPController>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (!hpController.GetIsAlive() && hpController != null)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
            velocity.y = yNegativeVelocity;

        //movement + sprint
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
            timeWalking += Time.deltaTime;
        else
            timeWalking = 0;

        movement = transform.right * x + transform.forward * z;

        //jump + artificial gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        

        Inputs();
        Movement();
        Crouch();
        HeadBobbing();
    }
    void Inputs()
    {
        //movement
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isSprinting = true;
                isCrouched = false;
            }
            else
                isSprinting = false;

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


#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
            UnityEditor.EditorApplication.isPlaying = false;
#endif 
    }
    void Movement()
    {
        if(isSprinting)
            controller.Move(movement * sprintSpeed * Time.deltaTime);
        if (isCrouched)
            controller.Move(movement * crouchMovementSpeed * Time.deltaTime);
        if (!isSprinting && !isCrouched)
            controller.Move(movement * speed * Time.deltaTime);
    }
    void Crouch()
    {
        float height = cam.position.y;
        if (isCrouched && height > CrouchedHead.position.y + 0.5f)
            height -= Time.deltaTime * crouchSpeed;


        cam.position = new Vector3(cam.position.x, height, cam.position.z);
    }

    void HeadBobbing()
    {
        Vector3 newHeadPosition;
        if (!isCrouched)
            newHeadPosition = Head.position + CalculateHeadBobOffset(timeWalking);
        else
            newHeadPosition = CrouchedHead.position + CalculateHeadBobOffset(timeWalking);

        if (isSprinting && !isCrouched)
        {
            hBobFrequency = sprintingHeadBobFrequency;
            hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
        }
        else if (!isSprinting && isCrouched)
        {
            hBobFrequency = walkingHeadBobFrequency;
            hBobVerticalAmplitude = crouchingHeadVerticalAmplitude;
        }
        else
        {
            hBobFrequency = walkingHeadBobFrequency;
            hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
        }

        if (isGrounded)
        {
            cam.position = Vector3.Lerp(cam.position, newHeadPosition, headBobbingIntensity);
            if ((cam.position - newHeadPosition).magnitude <= axisDifference)
            {
                cam.position = newHeadPosition;
            }
        }


    }
    Vector3 CalculateHeadBobOffset(float value)
    {
        float movement;
        Vector3 offset = Vector3.zero;
        if (value > 0)
        {
            movement = Mathf.Sin(value * hBobFrequency * 2) * hBobVerticalAmplitude;
            if (!isCrouched)
                offset = Head.transform.up * movement;
            else
                offset = CrouchedHead.transform.up * movement;
        }
        return offset;
    }
}
