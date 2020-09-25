using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform RelativeObject;
    public float WalkSpeed = 3f;
    public float SideStepSpeed = 2f;
    public float SprintSpeed = 6f;
    public float AccelerationSpeed = 2f;
    public float DeAccelerationSpeed = 4f;
    private Vector3 currentMoveSpeed;

    private Rigidbody rb;
    public float Stamina = 5f;
    public float StaminaRegenDelay = 2f;
    private float currentStamina;
    private float currentStaminaRegenDelay;
    

    private bool sprinting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        currentStamina = Stamina;
        currentStaminaRegenDelay = Stamina;
    }

    private void Update()
    {
        if (Input.GetButton("Sprint"))
            sprinting = true;
        else
            sprinting = false;

        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Move(moveInput);
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector3(0, Physics.gravity.y, 0));
    }

    private void Move(Vector3 moveInput)
    {
        Vector3 targetSpeed = Vector3.zero;
        Vector3 movement = Vector3.zero;

        if (moveInput.x != 0)
        {
            targetSpeed.x = moveInput.x * SideStepSpeed;
            currentMoveSpeed.x = Mathf.Lerp(currentMoveSpeed.x, targetSpeed.x, AccelerationSpeed * Time.deltaTime);
        }
        else
        {
            currentMoveSpeed.x = Mathf.Lerp(currentMoveSpeed.x, targetSpeed.x, DeAccelerationSpeed * Time.deltaTime);
        }

        if (moveInput.z != 0)
        {
            targetSpeed.z = moveInput.z * WalkSpeed;
            if (sprinting && moveInput.z > 0 && currentStamina > 0)
            {
                targetSpeed.z = moveInput.z * SprintSpeed;

                currentStamina -= Time.deltaTime;
                currentStaminaRegenDelay = 0;
            }

            currentMoveSpeed.z = Mathf.Lerp(currentMoveSpeed.z, targetSpeed.z, AccelerationSpeed * Time.deltaTime);
        }
        else
        {
            currentMoveSpeed.z = Mathf.Lerp(currentMoveSpeed.z, targetSpeed.z, DeAccelerationSpeed * Time.deltaTime);
        }

        movement.x = currentMoveSpeed.x;
        movement.z = currentMoveSpeed.z;

        movement = RelativeObject.TransformDirection(movement);

        rb.position += movement * Time.deltaTime;

        if (currentStaminaRegenDelay >= StaminaRegenDelay)
        {
            currentStaminaRegenDelay = 2;
            if (!sprinting || moveInput.z <= 0)
            {
                if (currentStamina < Stamina)
                    currentStamina += Time.deltaTime;
            }
        }
        else
        {
            if (!sprinting || moveInput.z <= 0)
                currentStaminaRegenDelay += Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, Stamina);
        currentStaminaRegenDelay = Mathf.Clamp(currentStaminaRegenDelay, 0, StaminaRegenDelay);
    }
}
