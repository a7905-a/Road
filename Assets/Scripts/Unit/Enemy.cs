using System;
using UnityEngine;

namespace ProjectRoad.Unit
{
    public class Enemy : BaseUnit
    {
        [Header("VFX 위치")]
        [SerializeField] private Transform damagePoint;

        [Header("시각 효과")]
        [SerializeField] private UnitVisualData visualData;
        

        protected override void OnHit()
        {
            if (visualData != null && damagePoint != null)
            {
                Instantiate(visualData.hitEffect, damagePoint.position, damagePoint.rotation);
            }
        }

    }
}

