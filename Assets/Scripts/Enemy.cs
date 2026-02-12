using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyStateManager enemyStateManager; 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyStateManager.currentTarget = other.transform;
        }
    }

}

