using UnityEngine;

[CreateAssetMenu(menuName =  "Unit/Unit Visual Data")]
public class UnitVisualData : ScriptableObject
{
    [TextArea]
    public string UnitDescription;

    [Header("타격 이펙트")]
    public ParticleSystem hitEffect;

    [Header("발사 이펙트")]
    public ParticleSystem muzzleFlash;
    public GameObject bulletLine;

    // public float AttackRate;
    // public float AttackRange;
}
