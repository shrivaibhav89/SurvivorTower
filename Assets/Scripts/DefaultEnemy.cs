using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : Enemy
{
    private void Start()
    {
        base.Start();
        agent.speed = agentSpeed;
    }
}
