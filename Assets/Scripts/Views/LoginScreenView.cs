using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms;
using DapperDino.DapperTools.StateMachines;

public class LoginScreenView : BaseView
{
    [SerializeField]
    ObjectValueList allCategoriesResultList;
    [SerializeField]
    VoidEvent E_GetAllProductsRequest;
    [SerializeField]
    VoidEvent E_GetAllCategoriesRequest;
    [SerializeField]
    ObjectValueList allProductResultList;
    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        /*if (allCategoriesResultList.List.Count <= 0)
        {
            //Debug.Log("FETCHING CATEGORY DATA BEFORE LOADING FURNITURE.......!!!");
            E_GetAllCategoriesRequest.Raise();
        }
        if (allProductResultList.List.Count <= 0)
        {
            Debug.Log("FETCHING Product DATA BEFORE LOADING FURNITURE.......!!!");
            E_GetAllProductsRequest.Raise();
        }*/
    }
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}
