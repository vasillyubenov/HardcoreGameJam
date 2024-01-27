using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, IPunchable
{
    public void GetHit(Attack.AttackType attack, float knockback, float damage, float stun)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2 (knockback, Random.value * 0.5f);
    }
}
