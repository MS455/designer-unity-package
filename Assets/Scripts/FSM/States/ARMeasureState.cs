using JSAM;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;

public class ARMeasureState : State
{
    // Start is called before the first frame update
    [SerializeField]
    ARMeasure arMeasure;

    public override void Enter()
    {
        base.Enter();
        arMeasure.StartMeasure(1);

    }
    public override void Exit()
    {
        base.Exit();
    }



}
