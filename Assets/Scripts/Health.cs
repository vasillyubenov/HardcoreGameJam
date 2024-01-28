using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private UnityEvent onDeath;
    [SerializeField] private float maxHealth = 100, health;

    [SerializeField] public bool immune = false;

    [SerializeField] private Attack controller;

    private void Start()
    {
        health = maxHealth;
        if (controller != null) controller.onBlockEvent += Block;
    }
    public void TakeDamage(float value)
    {
        health -= value;

        if (health < 0) onDeath.Invoke();
    }
    public void Block(bool state)
    {
        immune = state;

        Debug.Log(state);
    }
    public void Revive()
    {
        health = maxHealth;
    }
}
