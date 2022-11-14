using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;
using TMPro;

public class QuotationView : BaseView
{
    [SerializeField]
    GameObject BasicinfoForm;
    [SerializeField]
    GameObject QuotationPage;
    [SerializeField]
    StringVariable v_PreviousState;

    // Start is called before the first frame update
    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        BasicinfoForm.SetActive(true);
        QuotationPage.SetActive(false);
    }
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
        v_PreviousState.Value = "Quotation";
    }
}
