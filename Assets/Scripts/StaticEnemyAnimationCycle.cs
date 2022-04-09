using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyAnimationCycle : StateMachineBehaviour
{

    public float maxTimer = 10;
    public float minTimer = 3;

    float randomMinTimer = 3;
    bool calculatedMinTimer = false; 

    float timer = 0;
    string[] triggers = {
        "Warrior",
        "Happy",
        "Offensive"
    };

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!calculatedMinTimer) {
            randomMinTimer = Random.Range(minTimer, maxTimer);
            calculatedMinTimer = true;
            Debug.Log("Calculating");
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("Idle")) {
            timer += Time.deltaTime;

            int random = Random.Range(0, 11);
            if (timer >= randomMinTimer && (timer >= maxTimer || random % 3 == 0)) {
                int randomTrigger = Random.Range(0, triggers.Length);
                animator.SetTrigger(triggers[randomTrigger]);
            }
        } else {
            timer = 0;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
