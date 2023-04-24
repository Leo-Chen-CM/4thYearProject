using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class CheckUnitOwnership : Node
{
    private bool _unitIsMine;

    //public CheckUnitOwnership(RTSGameController manager) : base()
    //{
    //    _unitIsMine = manager.m_selectedUnits.== GameManager.instance.gamePlayersParameters.myPlayerId;
    //}

    public override NodeState Evaluate()
    {
        state = _unitIsMine ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}
