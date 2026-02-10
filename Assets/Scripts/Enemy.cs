using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f; 
    


    void Update()
    {
        //if (player == null) return;

        // if ( DistanceToPlayer() <= chaseDistance)
        // {
        //     Debug.Log("추적 시작!");
        // }
    }

    // float DistanceToPlayer()
    // {
    //     GameObject player = GameObject.FindWithTag("Player");
    //     return Vector3.Distance(transform.position, player.transform.position);
    // }
}

