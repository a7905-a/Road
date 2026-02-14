using UnityEngine;

[CreateAssetMenu(menuName =  "Unit/Unit Data")]
public class UnitData : ScriptableObject
{
    public string UnitName;
    [TextArea]
    public string UnitDescription;

    public float MaxHealth;
    public float Damage;
    public float AttackRate;
    public float AttackRange;
}
