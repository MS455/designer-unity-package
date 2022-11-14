using System.Collections;
using System.Collections.Generic;
using DapperDino.DapperTools.StateMachines;
using UnityEngine;

public class CatalogStateView : BaseView
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> ChildPanels;
    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        foreach(var child in ChildPanels)
        {
            child.SetActive(false);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}
