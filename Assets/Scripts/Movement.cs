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
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource walkingAudioSource;

    private Vector2 input;
    private Rigidbody2D playerRB;
    private bool isGrounded;
    private bool canDoubleJump;
    private float directionX = 1;
    private float dashTimePassed = 0;
    private bool isDashing;
    private float cooldownTimeDash;
    private bool isStunned;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        dashParticles.gameObject.SetActive(false);
    }
    public void Stun(bool state)
    {
        isStunned = state;
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
        else
        {
            animator.SetBool("isWalking", false);
            Debug.Log("Stopping");
            SoundManager.Instance.StopMove(walkingAudioSource);
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

        if (isStunned)
        {
            return;
        }

        animator.SetBool("isWalking", true);
        SoundManager.Instance.PlayMove(walkingAudioSource);

        playerRB.velocity = new Vector2(input.x * speed, playerRB.velocity.y);
        directionX = Mathf.Sign(input.x);
        transform.localScale = new Vector3(directionX, 1, 1);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isStunned)
        {
            return;
        }

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

        animator.SetTrigger("jump");
        SoundManager.Instance.PlayJump();

        playerRB.AddForce(Vector2.up * jumpForce);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (isStunned)
        {
            return;
        }

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

        animator.SetTrigger("dash");
        SoundManager.Instance.PlayDash();

        cooldownTimeDash = dashCooldown;
        isDashing = true;
        dashParticles.gameObject.SetActive(true);
        dashTimePassed = dashDuration;
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        playerRB.AddForce(Vector2.right * dashForce * directionX, ForceMode2D.Impulse);
    }
}
