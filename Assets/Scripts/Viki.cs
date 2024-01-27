using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viki : MonoBehaviour
{
    enum MovementMode
    {
        Platformer,
        Strafe
    }

    [SerializeField]
    private MovementMode _movementMode = MovementMode.Strafe;
    [SerializeField]
    private float _walkSpeed = 3f;
    [SerializeField]
    private float _runningSpeed = 60f;
    [SerializeField]
    private float _timeOfDash = 50;
    [SerializeField]
    private float _gravity = 9.81f;
    [SerializeField]
    private float _gravityPlatformer = -12f;
    [SerializeField]
    private float _jumpSpeed = 3.5f;
    [SerializeField]
    private float _doubleJumpMultiplier = 0.5f;
    [SerializeField]
    private GameObject _cameraRig;

    public float jumpHeight = 1;

    private CharacterController _controller;

    private float _directionY;
    private float _currentSpeed;

    private bool _canDoubleJump = false;
    private bool _canDash = false;
    private long _lastDashTime = -1000;
    System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;

    private float velocityY;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        timer.Start();
    }

    void Update()
    {
        if (_movementMode == MovementMode.Strafe)
        {
            MovementStafe();
        }
        if (timer.ElapsedMilliseconds - _lastDashTime >= 1000)
        {
            _canDash = true;
        }
    }

    private void LateUpdate()
    {
        if (_cameraRig != null && IsPlayerMoving() && _movementMode == MovementMode.Strafe)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _cameraRig.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    private bool IsPlayerMoving()
    {
        return Input.GetAxisRaw("Horizontal") != 0;
    }

    private void MovementStafe()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            _canDash = false;
            _lastDashTime = timer.ElapsedMilliseconds;
        }

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        if (_controller.isGrounded)
        {
            _directionY = 0;
            _canDoubleJump = true;

            if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0)
            {
                _directionY = _jumpSpeed;
            }
        }
        else
        {
            if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0 && _canDoubleJump)
            {
                _directionY = _jumpSpeed * _doubleJumpMultiplier;
                _canDoubleJump = false;
            }
        }

        float targetSpeed = (timer.ElapsedMilliseconds - _lastDashTime < _timeOfDash) ? _runningSpeed : _walkSpeed;
        _currentSpeed = (timer.ElapsedMilliseconds - _lastDashTime < _timeOfDash) ? _runningSpeed : Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        if (timer.ElapsedMilliseconds - _lastDashTime < _timeOfDash*5)
        {
            _directionY = 0;

            moveDirection = transform.TransformDirection(moveDirection);

            _controller.Move(_currentSpeed * Time.deltaTime * moveDirection);
        }
        else
        {
            _directionY -= _gravity * Time.deltaTime;

            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection.y = (timer.ElapsedMilliseconds - _lastDashTime < _timeOfDash) ? 0f : _directionY;

            _controller.Move(_currentSpeed * Time.deltaTime * moveDirection);
        }
    }
}
