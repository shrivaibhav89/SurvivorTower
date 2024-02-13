using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlowEnemy : Enemy
{
   private void Start()
    {
        base.Start();
        agent.speed = agentSpeed;
        health = 3f;
        transform.localScale = new Vector3(2, 2, 2); // Assuming the enemy is twice as big
    }
}
