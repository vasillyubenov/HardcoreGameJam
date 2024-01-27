using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float dashForce = 0.5f;
    [SerializeField] private float jumpForce = 0.5f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private ParticleSystem dashParticles;
    
    private Vector2 input;
    private Rigidbody2D playerRB;
    private bool isGrounded;
    private bool canDoubleJump;
    private float directionX = 1;
    private float dashTimePassed = 0;
    private bool isDashing;
    private float cooldownTimeDash;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        dashParticles.gameObject.SetActive(false);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        if (input == Vector2.zero)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x / speed, playerRB.velocity.y);
        }
    }

    public void FixedUpdate()
    {
        if (input != Vector2.zero)
        {
            Move();
        }

        if (dashTimePassed > 0)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        }
    }

    private void Update()
    {
        if (dashTimePassed > 0)
        {
            dashTimePassed -= Time.deltaTime;
        }
        else
        {
            if (isDashing)
            {
                playerRB.velocity = new Vector2(0, 0);
            }
            isDashing = false;
            dashParticles.gameObject.SetActive(false);
        }

        if (cooldownTimeDash > 0)
        {
            cooldownTimeDash -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            isGrounded = true;
            canDoubleJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            isGrounded = false;
        }
    }

    public void Move()
    {
        if (isDashing)
        {
            return;
        }

        playerRB.velocity = new Vector2(input.x * speed, playerRB.velocity.y);
        directionX = Mathf.Sign(input.x);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started && context.performed)
        {
            return;
        }
        else if (context.canceled)
        {
            return;
        }

        if (context.ReadValue<float>() == 0)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }

        if (!isGrounded && !canDoubleJump)
        {
            return;
        }

        if (isGrounded)
        {
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
            canDoubleJump = false;
        }

        playerRB.AddForce(Vector2.up * jumpForce);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.started && context.performed)
        {
            return;
        }
        else if (context.canceled)
        {
            return;
        }

        if (dashTimePassed > 0)
        {
            return;
        }

        if (cooldownTimeDash > 0)
        {
            return;
        }

        cooldownTimeDash = dashCooldown;
        isDashing = true;
        dashParticles.gameObject.SetActive(true);
        dashTimePassed = dashDuration;
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        playerRB.AddForce(Vector2.right * dashForce * directionX, ForceMode2D.Impulse);
    }
}
