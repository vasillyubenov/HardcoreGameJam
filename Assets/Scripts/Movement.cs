using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    
    private Vector2 input;

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        if (input != Vector2.zero)
        {
            Move();
        }
    }

    public void Move()
    {
        Vector2 deltaMovement = input * speed * Time.deltaTime;
        transform.position += new Vector3(deltaMovement.x, deltaMovement.y, 0);
    }
}
