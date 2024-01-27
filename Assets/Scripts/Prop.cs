using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, IPunchable
{
    public void GetHit(Attack.AttackType attack, float knockback, float damage, float stun, int teamTag)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2 (knockback, knockback);
    }
}
