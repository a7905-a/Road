using UnityEngine;

public class EnemyDateManger : MonoBehaviour
{
    [SerializeField] EnemyData _enemyDate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetEnemyDate();
    }

    void GetEnemyDate()
    {
        print($"Name : {_enemyDate.EnemyName}\n" +
              $"Description : {_enemyDate.EnemyDescription}\n" +
              $"MaxHP : {_enemyDate.MaxHP}\n" +
              $"Damage : {_enemyDate.Damage}\n" +
              $"MoveSpeed : {_enemyDate.MoveSpeed}");
    }
}
