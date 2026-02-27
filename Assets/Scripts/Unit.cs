using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit : BaseUnit
{
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform hitPoint;
    
    protected override void Start()
    {
        base.Start();
        UnitSelectionManager.Instance.AddUnit(gameObject);
    }

    void Update()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

    }

    protected override void Retire()
    {
        UnitSelectionManager.Instance.RemoveUnit(gameObject);
        base.Retire();
    }

    protected override void OnHit()
    {
        if (hitPoint != null)
        {
            Instantiate(unitData.hitEffect, hitPoint.position, hitPoint.rotation);
        }
    }

    public void AttackEffect()
    {
        Instantiate(unitData.muzzleFlash, attackPoint.position, attackPoint.rotation);
    }

}
