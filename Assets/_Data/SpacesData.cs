using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpacesData
{
    public int orderID;
    public int quotationPrice;
    public string spaceName;
    public string orderStatus;
    public List<LayoutData> LayoutDataList = new List<LayoutData>();
    public CustomerData customerData = new CustomerData();
    
    public SpacesData()
    {
        orderID = 0;
        quotationPrice = 0;
        spaceName = string.Empty;
        orderStatus = string.Empty;
        LayoutDataList = new List<LayoutData>();
        customerData = new CustomerData();
    }

   /* public SpacesData(int orderID,int quotationPrice,string spaceName, string orderStatus,List<LayoutData> spaceDataList,CustomerData customerData)
    {
        this.orderID = orderID;
        this.quotationPrice = quotationPrice;
        this.spaceName = spaceName;
        this.orderStatus = orderStatus;
        this.LayoutDataList = spaceDataList;
        this.customerData = customerData;
    }*/


    public LayoutData GetLayoutData(int index)
    {
        if (index >= 0 && index < LayoutDataList.Count)
            return LayoutDataList[index];

        return null;

    }


    public static SpacesData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SpacesData>(jsonString);
    }

    public string SaveToString()
    {
        string hniSpaceData =  JsonUtility.ToJson(this);
        PlayerPrefs.SetString("HNIspacesData", hniSpaceData);
        return hniSpaceData;
    }
    
}

