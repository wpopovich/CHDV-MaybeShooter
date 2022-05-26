using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        InitEnemy();
    }

    protected override void Update()
    {
        if (isDead)
            return;

        KillEnemy(killed);
    }

}
