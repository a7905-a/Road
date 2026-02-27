using UnityEngine;

[CreateAssetMenu(menuName =  "Unit/Unit Data")]
public class UnitData : ScriptableObject
{
    public string UnitName;
    [TextArea]
    public string UnitDescription;

    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public float MaxHealth;
    public float Damage;
    public float AttackRate;
    public float AttackRange;
}
