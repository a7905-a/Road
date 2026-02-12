using UnityEngine;


[CreateAssetMenu(menuName =  "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    [TextArea]
    public string EnemyDescription;

    public float MaxHP;
    public float Damage;
    public float AttackRange;
}
