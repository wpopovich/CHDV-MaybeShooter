using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        InitEnemy();
    }

    void Update()
    {
        if (isDead)
            return;

        KillEnemy(killed);
    }

}
