using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private UnityEditor.Animations.AnimatorController animationController;

    [SerializeField] private HitData lightAttack, heavyAttack;

    [SerializeField] private PunchCollider punchCollider;

    public delegate void OnHitActivated(HitData args);
    public event OnHitActivated onHitEvent;

    private bool canAct = true;

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
    private void KeyboardTestInput()
    {
        if (canAct)
        {
            if (Input.GetKeyDown(KeyCode.Q)) SendPunch(AttackType.light);
            else if (Input.GetKeyDown(KeyCode.E)) SendPunch(AttackType.heavy);
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

[CreateAssetMenu(fileName = "New Hit", menuName = "Create New Hit Properties")]
public class HitData : ScriptableObject
{
    public Attack.AttackType attackType = Attack.AttackType.light;

    [Header("Time Properties")]
    public float windup = 0.1f;
    public float duration = 0.1f;
    public float recoil = 0.1f;

    [Header("Impact Properties")]
    public float range = 10f;
    public float knockback = 10f;
    public float damage = 10f;
    public float stunTime = 0.5f;

    public float GetAttackTime()
    {
        return windup + duration + recoil;
    }
}