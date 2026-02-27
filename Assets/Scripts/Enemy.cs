using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : BaseUnit
{
    [SerializeField] Transform damagePoint;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnHit()
    {
        if (damagePoint != null)
        {
            Instantiate(unitData.hitEffect, damagePoint.position, damagePoint.rotation);
        }
    }
    protected override void Retire()
    {
        base.Retire();
    }

}

