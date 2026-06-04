using System;
using UnityEngine;
using ProjectRoad.Manager;

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
                //Instantiate(visualData.hitEffect, hitPoint.position, hitPoint.rotation);
                GameObject hitEffect = PoolManager.instance.ActiveObject(0);
                PoolManager.instance.SetPosition(hitEffect, hitPoint.position);
            }
        }

        public void AttackEffect()
        {
            if (visualData.muzzleFlash != null)
            {
                //Instantiate(visualData.muzzleFlash, attackPoint.position, attackPoint.rotation);
                GameObject muzzleFlash = PoolManager.instance.ActiveObject(1);
                PoolManager.instance.SetPosition(muzzleFlash, attackPoint.position);
            }
        }

    }
}

