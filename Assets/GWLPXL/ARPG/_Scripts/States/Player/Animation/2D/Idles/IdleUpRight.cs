﻿
using UnityEngine;

using System;


namespace GWLPXL.ARPGCore.States.com
{

    
   

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Animation/Idle UpRight")]

    public class IdleUpRight : PlayerMovementState2D
    {


        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            if (forEntity.GetActorHub().PlayerControlled != null)
            {
                return forEntity.GetActorHub().InputHub.MoveInputs.GetHorizontalRaw() == 0 && forEntity.GetActorHub().InputHub.MoveInputs.GetVerticalRaw() == 0 && forEntity.Get2D().GetFacingDirection() == GlobalFacingDirection.UpRight;
            }
            else
            {
                return forEntity.GetAI().GetDirection().x == 0 && forEntity.GetAI().GetDirection().y == 0 && forEntity.Get2D().GetFacingDirection() == GlobalFacingDirection.UpRight;

            }

        }

        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            AnimateStop state = new AnimateStop(forEntity, Vars);
            Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasWalkingTarget());
            stateDic.Add(forEntity, state);
        }
    }


}