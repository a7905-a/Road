using System;
using UnityEngine;
using UnityEngine.AI;

public class Unit : BaseUnit
{

    protected override void Start()
    {
        base.Start();
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
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

        Debug.Log(currentHealth);
    }

    protected override void Retire()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
        base.Retire();
    }

}
