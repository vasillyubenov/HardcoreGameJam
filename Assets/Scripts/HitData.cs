using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
