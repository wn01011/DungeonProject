using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Attack : StateMachineBehaviour
{
    private KnightBehaviour knightBehaviour = null;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        knightBehaviour = animator.gameObject.GetComponent<KnightBehaviour>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        knightBehaviour.isAttack = false;
    }
}
