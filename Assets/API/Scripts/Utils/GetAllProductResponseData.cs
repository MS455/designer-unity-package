using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class GetAllProductResponseData
{
    [Serializable]
    public class ProductData
    {
        public string IosSrc;
        public string Src;
        public string DisplayProductNumber;
        public string Description;
        public string Model_ID;
        public string Color;
        public ColorCode ColorCode;
        public int Volume;
        public string Usage;
        public double Length;
        public string SubCategory;
        public double Height;
        public string Product_Image;
        public string PriceText;
        public int Price;
        public double Area;
        public double Width;
        public string Name;
        public string Short_Description;
        public List<string> variants;
        public List<Dimension> dimensions;
    }

    [Serializable]
    public class ColorCode
    {
        public int R;
        public int G;
        public int B;
    }

    [Serializable]
    public class Dimension
    {
        public double Length;
        public double Width;
        public double Height;
        public string Section;
    }

    [Serializable]
    public class ProductDataList
    {
        public List<ProductData> value;
    }

    public ProductDataList result = new ProductDataList();

    public static GetAllProductResponseData CreateFromJSON(string jsonString)
    {

        return JsonUtility.FromJson<GetAllProductResponseData>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void Addproduct(string productName, string productImage , string category ,double productArea)
    {
        ProductData productData = new ProductData();

        productData.Name = productName;
        productData.Product_Image = productImage;
        productData.Area = productArea;
        productData.SubCategory = category;

        result.value.Add(productData);
        
    }
}
