using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PunchCollider : MonoBehaviour
{
    [SerializeField] private Attack controller;

    [SerializeField] private Vector2 hitOffset;

    [SerializeField] private int teamTag;

    private bool activeHit = false, hasHit = false;

    private HitData hitData;

    private void Start()
    {
        controller.onHitEvent += PerformPunch; 
    }

    private void PerformPunch(HitData args)
    {
        transform.localPosition = new Vector3(args.range / 10f + hitOffset.x, hitOffset.y, 0);
        hitData = args;

        StartCoroutine(HitCoroutine(args));
    }
    private IEnumerator HitCoroutine(HitData args)
    {
        hasHit = false;

        yield return new WaitForSeconds(args.windup);

        activeHit = true;

        yield return new WaitForSeconds(args.duration);

        activeHit = false;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (activeHit && !hasHit)
        {
            IPunchable hit;
            int direction = coll.transform.position.x < controller.transform.position.x ? -1 : 1;

            if (coll.TryGetComponent(out hit))
            {
                hit.GetHit(hitData.attackType, hitData.knockback * direction, hitData.damage, hitData.stunTime, teamTag);

                hasHit = true;
            }
        }
    }
}
public interface IPunchable
{
    public void GetHit(Attack.AttackType type, float knockback, float damage, float stun, int teamTag);
}
