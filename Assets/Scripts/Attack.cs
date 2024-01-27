using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private UnityEditor.Animations.AnimatorController animationController;

    [Header("Light Attack")]
    [SerializeField] float windupLightAttack = 0.1f;
    [SerializeField] float durationLightAttack = 0.1f;
    [SerializeField] float postLightAttackDelay = 0.1f;
    [SerializeField] float rangeAttackLight = 10f;
    [SerializeField] float attackPowerLight = 10f;
    [SerializeField] CapsuleCollider2D lightAttackCollider;
    
    [Header("Heavy Attack")]
    [SerializeField] float windupHeavyAttack = 0.3f;
    [SerializeField] float durationHeavyAttack = 0.1f;
    [SerializeField] float postHeavyAttackDelay = 0.3f;
    [SerializeField] float rangeAttackHeavy = 30f;
    [SerializeField] float attackPowerHeavy = 30f;
    [SerializeField] CapsuleCollider2D heavyAttackCollider;

    private float windupTime = 0;
    private float postAttackDelay = 0;
    private float attackDuration;
    private float attackRange;
    private float attackPower;
    private CapsuleCollider2D attackCollider;
    private float tempAttackDuration;
    private Vector3 initialColliderPosition;

    public void OnUseLightAttack()
    {
        if (!CanUseAttack())
        {
            return;
        }

        Debug.Log("Light attack");
        attackDuration = durationLightAttack;
        postAttackDelay = postLightAttackDelay;
        windupTime = windupLightAttack;
        attackRange = rangeAttackLight;
        attackCollider = lightAttackCollider;
        attackPower = attackPowerLight;

        StartCoroutine(UseAttack());
    }

    public void OnUseHeavyAttack()
    {
        if (!CanUseAttack())
        {
            return;
        }

        attackDuration = durationHeavyAttack;
        postAttackDelay = postHeavyAttackDelay;
        windupTime = windupHeavyAttack;
        attackRange = rangeAttackHeavy;
        attackCollider = heavyAttackCollider;
        attackPower = attackPowerHeavy;

        StartCoroutine(UseAttack());
    }

    private IEnumerator UseAttack()
    {
        tempAttackDuration = 0;
        initialColliderPosition = attackCollider.transform.position;
        yield return new WaitForSeconds(windupTime);

        if (!CanUseAttack())
        {
            yield break;
        }        
    }

    private bool CanUseAttack()
    {
        return windupTime <= 0 || postAttackDelay <= 0;
    }

    private void Update()
    {
        if (windupTime > 0)
        {
            windupTime -= Time.deltaTime;
            return;
        }

        if (attackCollider != null)
        {
            tempAttackDuration += Time.deltaTime;
            Vector3 interpolatedPosition = Vector3.Lerp(attackCollider.transform.position, attackCollider.transform.position + Vector3.right * attackRange, Mathf.Min(tempAttackDuration / attackDuration, 1));
            attackCollider.transform.position = interpolatedPosition;

            if (tempAttackDuration >= attackDuration)
            {
                attackCollider.transform.position = initialColliderPosition;
                attackCollider = null;
                tempAttackDuration = 0;
            }
        }
    }
}