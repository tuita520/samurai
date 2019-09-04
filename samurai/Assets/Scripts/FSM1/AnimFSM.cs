using Phenix.Unity.AI;
using System.Collections.Generic;

public class AnimFSM : FSM
{
    public AnimFSM(List<FSMState> states, FSMState defState) : base(states, defState) { }
}