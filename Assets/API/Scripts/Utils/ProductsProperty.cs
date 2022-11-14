using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductsProperty : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    string product_Name;

    [SerializeField]
    string product_thumbnailUrl;

    [SerializeField]
    string productDescription;

    [SerializeField]
    string modelId;

    [SerializeField]
    double productArea;
    [SerializeField]
    double length;
    [SerializeField]
    double width;

    [SerializeField]
    string displayProductNumber;

    [SerializeField]
    double price;

    public string Product_Name { get => product_Name; set => product_Name = value; }
    public string Product_thumbnailUrl { get => product_thumbnailUrl; set => product_thumbnailUrl = value; }
    public string Product_description { get => productDescription; set => productDescription = value; }
    public double Product_Area { get => productArea; set => productArea = Mathf.Round((float)(value / 30.8)); }
    public double product_Length { get; set; }
    public double product_Width { get; set; }
    public string DisplayProductNumber { get => displayProductNumber; set => displayProductNumber = value; }
    public string ModelID { get => modelId; set => modelId = value; }
    public double Price { get => price; set => price = value; }
    
}

