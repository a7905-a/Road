using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    float unitHealth;
    public float unitMaxHealth;

    public HealthTracker healthTracker;

    void Start()
    {
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);

        unitHealth = unitMaxHealth;
        UpdateHealthUI();
    }

    

    void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }

    void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);

        if (unitHealth <= 0)
        {
            // 유닛 기절 처리
            Destroy(gameObject);
        }   
    }

    internal void TakeDamage(int damageIoInflict)
    {
        unitHealth -= damageIoInflict;
        UpdateHealthUI();
    }

}
