using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour, IPunchable
{
    [SerializeField] private UnityEvent onLightHit, onHeavyHit;

    [SerializeField] private CameraScript cameraEffect;
    [SerializeField] private Health healthSystem;

    [SerializeField] private int teamTag;

    public void GetHit(Attack.AttackType attack, float knockback, float damage, float stun, int tag)
    {
        if (teamTag != tag)
        {
            if (healthSystem.immune)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(knockback / 5f, 0);

                healthSystem.TakeDamage(damage / 5f);
            }
            else
            {
                if (attack == Attack.AttackType.light) onLightHit.Invoke();
                else onHeavyHit.Invoke();

                GetComponent<Rigidbody2D>().velocity = new Vector2(knockback, Mathf.Abs(knockback));

                healthSystem.TakeDamage(damage);
            }

            if (cameraEffect != null) CameraEffects(knockback / 10f);
        }
    }
    private void CameraEffects(float shake)
    {
        cameraEffect.ShakeCamera(shake);
    }
}
