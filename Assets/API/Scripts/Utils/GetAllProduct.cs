using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class GetAllProduct
{
    [Serializable]
    public class Value
    {
        public string IosSrc;
        public string Src;
        public string DisplayProductNumber;
        public string Description;
        public string Model_ID;
        public string Color;
       
        public int Volume;
        public string Usage;
        public double Length;
        public string SubCategory;
        public double Height;
        public string Product_Image;
        public string PriceText;
        public int Price;
        public object Area;
        public double Width;
        public string Name;
        public string Short_Description;
    }

    public class Result
    {
        public List<Value> values;
    }

    public List<Result> results = new List<Result>();
    
    public static GetAllProduct CreateFromJSON(string jsonString)
    {
        var data = JsonUtility.FromJson<GetAllProduct>(jsonString);
        Debug.Log(data);
        return JsonUtility.FromJson<GetAllProduct>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    /*public void Addproduct(string productName, string productImage , string hoverUrl ,double productArea)
    {
        ProductData productData = new ProductData();

        productData.Name = productName;
        productData.Product_Image = productImage;
        productData.Area = productArea;

        result.productDataList.Add(productData);
        
    }*/
}
