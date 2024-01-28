using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Prop : MonoBehaviour, IPunchable
{
    [SerializeField] private UnityEvent onHit;
    public void GetHit(Attack.AttackType attack, float knockback, float damage, float stun, int teamTag)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2 (knockback, knockback / 5f) * 30, ForceMode2D.Impulse);

        onHit.Invoke();
    }
}
