using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuotationPanelView : MonoBehaviour
{

    [SerializeField]
    TMP_Text SpaceName;
    [SerializeField]
    TMP_Text ClientName;
    [SerializeField]
    TMP_Text BillingName;
    [SerializeField]
    TMP_Text Email;
    [SerializeField]
    TMP_Text Phone;
    [SerializeField]
    TMP_Text QuotePrice;

    [SerializeField]
    IntVariable vi_CurrentSpace;
    [SerializeField]
    IntVariable vi_CurrentLayout;

    [SerializeField]
    CustomerData customerData;
    [SerializeField]
    SpacesData hniSpacesData;

    [SerializeField]
    AllSpacesData allSpacesData;
    [SerializeField]
    VoidEvent E_ToSpaceManager;

    SpacesData spacesData;
    LayoutData layoutData;

    double total_price;

    
    private void OnEnable()
    {
        customerData = CustomerData.CreateFromJSON(PlayerPrefs.GetString("CustomerData"));
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
        GenerateQuotation();
    }

    public void GenerateQuotation()
    {
        
        
        ClientName.text = customerData.OrgName;
        BillingName.text = customerData.BillingName;
        Email.text = customerData.Email;
        Phone.text = customerData.PhNo;
        spacesData = allSpacesData.GetSpacesData(vi_CurrentSpace.Value);
        SpaceName.text = spacesData.spaceName;
        layoutData = spacesData.GetLayoutData(vi_CurrentLayout.Value);

        foreach (var x in layoutData.productList)
        {
            total_price += x.price;
        }


        QuotePrice.text = string.Format("Quoted price ${0}", total_price);
        Debug.Log(total_price);
    }

    public void PersistData()
    {
        //hniSpacesData = SpacesData.CreateFromJSON(PlayerPrefs.GetString("HNIspacesData"));
        allSpacesData.GetSpacesData(vi_CurrentSpace.Value).customerData = customerData;
        allSpacesData.SaveToString();
        /*if (string.IsNullOrEmpty(PlayerPrefs.GetString("AllSpaceData")))
        {
            allSpacesData.spacesDatas.Add(hniSpacesData);
            allSpacesData.SaveToString();
        }
        else
        {
            allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
            allSpacesData.spacesDatas.Add(hniSpacesData);
            allSpacesData.SaveToString();
        }*/
    } 

    void GoToHome()
    {
        Debug.Log("Go Back Home");
        E_ToSpaceManager.Raise();
    }
}
