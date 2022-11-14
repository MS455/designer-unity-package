using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;
using TMPro;

public class BasicinfoView : MonoBehaviour
{
    [SerializeField]
    StringVariable V_CustomerData;

    [SerializeField]
    CustomerData customerData;

    [SerializeField]
    TMP_InputField BillingName;
    [SerializeField]
    TMP_InputField OrganizationName;
    [SerializeField]
    TMP_InputField Address;
    [SerializeField]
    TMP_InputField Email;
    [SerializeField]
    TMP_InputField PhoneNumber;

    [SerializeField]
    AllSpacesData allSpacesData;

    [SerializeField]
    IntVariable vi_currentspace;


    public void OnEnable()
    {
        gameObject.SetActive(true);
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
        customerData = allSpacesData.spacesDatas[vi_currentspace.Value].customerData;

        BillingName.text = string.IsNullOrEmpty(customerData.BillingName) ?  "": customerData.BillingName;
        OrganizationName.text = string.IsNullOrEmpty(customerData.OrgName) ?  "": customerData.OrgName ;
        Address.text = string.IsNullOrEmpty(customerData.Address) ?  "": customerData.Address ;
        PhoneNumber.text = string.IsNullOrEmpty(customerData.PhNo) ? "" : customerData.PhNo ;

    }
    public void OmDisable()
    {
        gameObject.SetActive(false);
    }
    public void submitData()
    {
        customerData.BillingName = BillingName.text;
        customerData.OrgName = OrganizationName.text;
        customerData.Address = Address.text;
        customerData.Email = Email.text;
        customerData.PhNo = PhoneNumber.text;
        customerData.SaveToString();
    }

    public void PersistData()
    {

    }
}
