using System;
using UnityEngine;

public class Enemy : BaseUnit
{
    [SerializeField] EnemyStateManager enemyStateManager;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyStateManager.currentTarget = other.transform;
        }
    }
    protected override void Retire()
    {
        base.Retire();
    }

}

