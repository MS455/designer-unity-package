using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GetAllProductResponse 
{
    [Serializable]
    public class ColorCode
    {
        public int R;
        public int G;
        public int B;
        public string _id;
    }

    [Serializable]
    public class Dimension
    {
        public double Length;
        public double Width;
        public double Height;
        public object Section;
        public string _id;
    }

    [Serializable]
    public class ProductData
    {
        public string _id;
        public string Name ;
        public string Description ;
        public string Usage ;
        public string Short_Description ;
        public string Color ;
        public string IosSrc ;
        public string Src ;
        public string Model_ID ;
        public double Volume ;
        public double Area ;
        public string Product_Image ;
        public int Price ;
        public string PriceText ;
        public double Length ;
        public double Width ;
        public double Height ;
        public string DisplayProductNumber ;
        public string SubCategory ;
        public List<object> variants ;
        public List<Dimension> dimensions ;
        public ColorCode ColorCode ;
        public DateTime createdAt ;
        public DateTime updatedAt ;
        public int __v ;
    }

    public List<ProductData> result = new List<ProductData>();

    public static GetAllProductResponse CreateFromJSON(string jsonString)
    {

        return JsonUtility.FromJson<GetAllProductResponse>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void Addproduct(string productName, string productImage, string category, double productArea)
    {
        ProductData productData = new ProductData();

        productData.Name = productName;
        productData.Product_Image = productImage;
        productData.Area = productArea;
        productData.SubCategory = category;

        result.Add(productData);

    }


}
