using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private HitData lightAttack, heavyAttack;

    [SerializeField] private PunchCollider punchCollider;
    [SerializeField] private GameObject blockIndicator;
    [SerializeField] private GameObject attackIndicator;

    public delegate void OnHitActivated(HitData args);
    public event OnHitActivated onHitEvent;

    public delegate void OnBlockActivated(bool arg);
    public event OnBlockActivated onBlockEvent;

    private bool canAct = true, isStunned = false, isBlocking = false;

    public enum AttackType
    {
        light,
        heavy
    }

    public void Stun(bool state)
    {
        isStunned = state;
    }

    public void SendPunch(bool isLight)
    {
        if (canAct && !isStunned)
        {
            StartCoroutine(AttackIndicator());
            HitData hitData = isLight ? lightAttack : heavyAttack;

            onHitEvent.Invoke(hitData);

            canAct = false;

            float cooldown = hitData.GetAttackTime();
            Invoke("ResetCooldown", cooldown);

            // play animation
            if (isLight)
            {
                animator.SetTrigger("lightAttack");
                SoundManager.Instance.PlayLightAttack();
            }
            else
            {
                animator.SetTrigger("heavyAttack");
                SoundManager.Instance.PlayHeavyAttack();
            }
        }
    }

    IEnumerator AttackIndicator()
    {
        attackIndicator.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        attackIndicator.SetActive(false);
    }

    public void Block(bool state)
    {
        if (canAct && !isStunned)
        {
            isBlocking = state;

            onBlockEvent.Invoke(state);
        }
    }
    private void KeyboardTestInput()
    {
        if (canAct && !isStunned)
        {
            if (Input.GetKeyDown(KeyCode.R)) Block(true);
            if (Input.GetKeyUp(KeyCode.R)) Block(false);

            if (!isBlocking)
            {
                if (Input.GetKeyDown(KeyCode.Q)) SendPunch(true);
                else if (Input.GetKeyDown(KeyCode.E)) SendPunch(false);
            }
        }
    }
    private void Update()
    {
        //KeyboardTestInput();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        float blockKeyValue = context.ReadValue<float>();
        isBlocking = blockKeyValue > 0;
        Debug.Log(isBlocking);
        onBlockEvent.Invoke(isBlocking);

        blockIndicator.SetActive(isBlocking);
    }

    private void ResetCooldown()
    {
        canAct = true;
    }
}