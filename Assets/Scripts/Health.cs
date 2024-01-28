using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private UnityEvent onDeath;
    [SerializeField] private float maxHealth = 100, health;

    [SerializeField] public bool immune = false;

    [SerializeField] private Attack controller;

    [SerializeField] private List<GameObject> lifesIcons;
    [SerializeField] private Slider bar;

    private int lifes = 3;

    private void Start()
    {
        health = maxHealth;
        if (controller != null) controller.onBlockEvent += Block;
        UpdateBar();
    }
    public void TakeDamage(float value)
    {
        health -= value;

        if (health < 0) {
            lifes--; 
            onDeath.Invoke(); 
            lifesIcons[lifes].SetActive(false); }


        UpdateBar();
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

    private void UpdateBar()
    {
        bar.value = health / maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "DeadZone")
        {
            TakeDamage(maxHealth);
        }
    }
}
