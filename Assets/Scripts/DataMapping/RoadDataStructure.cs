using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class RoadUnitData
{
    public int UnitID;
    public string UnitName;
    public float MaxHealth;
    public float Damage;
    public float AttackRate;
    public float AttackRange;
    public float MoveSpeed;
}

[Serializable]
public class UnitDataWrapper
{
    public List<RoadUnitData> unitList;
}

