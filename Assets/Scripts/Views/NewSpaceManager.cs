using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;

public class NewSpaceManager : BaseView
{
    [SerializeField]
    GameObject areaMeasurementPanel;
    [SerializeField]
    BoolEvent E_NewSpaceManager;


    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    public void ToggleARMeasurement(bool status)
    {
        Debug.Log("ToggleARMeasurement " + status);
        areaMeasurementPanel.SetActive(status);
        E_NewSpaceManager.Raise(status);
        gameObject.SetActive(!status);
    }
    
}


