using System;
using UnityEngine;

namespace ProjectRoad.Unit
{
    public class Enemy : BaseUnit
    {
        [Header("VFX 위치")]
        [SerializeField] private Transform attackPoint;
        [SerializeField] private Transform hitPoint;

        [Header("시각 효과 데이터")]
        [SerializeField] private UnitVisualData visualData;
        

        protected override void OnHit()
        {
            if (visualData != null && hitPoint != null)
            {
                Instantiate(visualData.hitEffect, hitPoint.position, hitPoint.rotation);
            }
        }

        public void AttackEffect()
        {
            if (visualData.muzzleFlash != null)
            {
                Instantiate(visualData.muzzleFlash, attackPoint.position, attackPoint.rotation);
            }
        }

    }
}

