using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private UnityEditor.Animations.AnimatorController animationController;

    [SerializeField] private HitData lightAttack, heavyAttack;

    [SerializeField] private PunchCollider punchCollider;

    public delegate void OnHitActivated(HitData args);
    public event OnHitActivated onHitEvent;

    public delegate void OnBlockActivated(bool arg);
    public event OnBlockActivated onBlockEvent;

    private bool canAct = true, isBlocking = false;

    public enum AttackType
    {
        light,
        heavy
    }
    private void SendPunch(AttackType attack)
    {
        HitData hitData = attack == AttackType.light ? lightAttack : heavyAttack;

        onHitEvent.Invoke(hitData);

        canAct = false;

        float cooldown = hitData.GetAttackTime();
        Invoke("ResetCooldown", cooldown);
    }
    private void Block(bool state)
    {
        isBlocking = state;

        onBlockEvent.Invoke(state);
    }
    private void KeyboardTestInput()
    {
        if (canAct)
        {
            if (Input.GetKeyDown(KeyCode.R)) Block(true);
            if (Input.GetKeyUp(KeyCode.R)) Block(false);

            if (!isBlocking)
            {
                if (Input.GetKeyDown(KeyCode.Q)) SendPunch(AttackType.light);
                else if (Input.GetKeyDown(KeyCode.E)) SendPunch(AttackType.heavy);
            }
        }
    }
    private void Update()
    {
        KeyboardTestInput();
    }

    private void ResetCooldown()
    {
        canAct = true;
    }
}