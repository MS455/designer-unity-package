using com.hexaware.datastructure;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LayoutData
{
    public string layoutName;
    public int layoutLength;
    public int layoutWidth;
    public List<sku> productList;
    public StoredPlacementInfo SavedDesign;

    [Serializable]
    public class sku
    {
        public string productName;
        public string ProductID;
        public int quantity;
        public string Thumbnail;
        public int area;
        public double price;


    }

    public void Copy ( LayoutData ld )
    {
        this.layoutLength = ld.layoutLength;
        this.productList = ld.productList;
        this.layoutName = ld.layoutName;
        this.layoutWidth = ld.layoutWidth;
    }

    public static LayoutData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LayoutData>(jsonString);

    }
   
    public string SaveToString()
    {
        string layoutDataString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("LayoutData", layoutDataString);
        return layoutDataString;
    }

}