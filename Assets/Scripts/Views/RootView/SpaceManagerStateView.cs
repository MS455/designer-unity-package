using System.Collections;
using System.Collections.Generic;
using DapperDino.DapperTools.StateMachines;
using UnityEngine;

public class SpaceManagerStateView : BaseView
{
    [SerializeField]
    GameObject SpaceManagerPanel;
    [SerializeField]
    List<GameObject> ChildPanels;
    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        DeactivateKids();

        SpaceManagerPanel.SetActive(true);
    }
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
    void DeactivateKids()
    {
        foreach(var x in ChildPanels)
        {
            x.SetActive(false);
        }
    }
}
