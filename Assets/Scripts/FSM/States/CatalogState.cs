using JSAM;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;
using UnityAtoms;
using System.Collections.Generic;
using System.Linq;

public class CatalogState : State
{
    [Header("Category Varaiables")]
    [SerializeField]
    VoidEvent E_GetAllCategoriesRequest;
    [SerializeField]
    ObjectValueList allCategoriesResultList;
    [SerializeField]
    VoidEvent E_GetAllCategoryOnHamburgerPanel;
    [Space]

    [Header("CATEGORY RESULT")]
    [SerializeField]
    GetAllCategoriesResponseData allCategoriesResultData;

    [SerializeField]
    GetAllProductResponse allProductResult;

    [Space]
    [Header("Product Variables")]
    [SerializeField]
    ObjectValueList allProductResultList;
    [SerializeField]
    VoidEvent E_GetAllproductONCatalog;

    [SerializeField]
    VoidEvent E_GetAllProductsRequest;

    [SerializeField]
    StringValueList AllFilterAppliedResultList;

    [SerializeField]
    GameObject ProductDescriptionPanel;
    [SerializeField]
    BoolVariable v_wasCartActive;

    public override void Enter()
    {
        base.Enter();
        v_wasCartActive.Value = false;
        if (allCategoriesResultList.List.Count <= 0)
        {
            //Debug.Log("FETCHING CATEGORY DATA BEFORE LOADING FURNITURE.......!!!");
            E_GetAllCategoriesRequest.Raise();
        }
        if (allProductResultList.List.Count <= 0)
        {
            Debug.Log("FETCHING Product DATA BEFORE LOADING FURNITURE.......!!!");
            E_GetAllProductsRequest.Raise();
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public void AllCategoriesResultRecievedSuccessful(string jsonString)
    {
        //Debug.Log("All Categories Result Recieved Successful -->\n" + jsonString);

        allCategoriesResultData = new GetAllCategoriesResponseData();
        allCategoriesResultData = GetAllCategoriesResponseData.CreateFromJSON(jsonString);
        GenerateCategoryResultList(allCategoriesResultData);
        E_GetAllCategoryOnHamburgerPanel.Raise();     
    }
    public void AllProductsResultRecievedSuccessful(string jsonString)
    {
        Debug.Log("All Products Result Recieved Successful -->\n" + jsonString);
        AllFilterAppliedResultList.Clear();  
        allProductResult = new GetAllProductResponse();
        allProductResult = GetAllProductResponse.CreateFromJSON(jsonString);
        GenerateProductResultList(allProductResult);

    }

    public void AllCategoriesResultRecievedFailed(string jsonString)
    {
        //Debug.Log("All Categories Result Recieved Failed -->\n" + jsonString);
     /*   E_ShowLoadingScreen.Raise(false);
        E_ActivateSNRPanel.Raise(true);*/
    }
    public void AllProductsResultRecievedFailed(string jsonString)
    {
        Debug.Log("All Categories Result Recieved Failed -->\n" + jsonString);
        /*   E_ShowLoadingScreen.Raise(false);
           E_ActivateSNRPanel.Raise(true);*/
    }

    void GenerateCategoryResultList(GetAllCategoriesResponseData allCategoriesResultData)
    {
        allCategoriesResultList.Clear();

        foreach (GetAllCategoriesResponseData.CategoryData categoryData in allCategoriesResultData.result)
        {
            var categoryModel = ScriptableObject.CreateInstance<Furniture_CategoryProperty>();
            categoryModel.Category_type = categoryData.type;
            //categoryModel.Category_thumbnailUrl = categoryData.url;
            //categoryModel.Category_thumbnailHoverUrl = categoryData.hoverUrl;

            allCategoriesResultList.Add(categoryModel);
        }
    }
    void GenerateProductResultList(GetAllProductResponse allProductsResultData)
    {
        Debug.Log(AllFilterAppliedResultList.Count);
        allProductResultList.Clear();

        if (AllFilterAppliedResultList.Count <= 0)
        {
            foreach (GetAllProductResponse.ProductData productData in allProductsResultData.result)
            {
                var ProductModel = ScriptableObject.CreateInstance<ProductsProperty>();
                ProductModel.Product_Name = productData.Name;
                ProductModel.Product_thumbnailUrl = productData.Product_Image;
                ProductModel.Product_Area = productData.Area;
                ProductModel.Product_description = productData.Description;
                ProductModel.product_Length = productData.Length;
                ProductModel.product_Width = productData.Width;
                ProductModel.DisplayProductNumber = productData.DisplayProductNumber;
                ProductModel.ModelID = productData.Model_ID;
                ProductModel.Price = productData.Price;

                allProductResultList.Add(ProductModel);
            }
        }
        else
        {
            var query = from productData in allProductsResultData.result
                        join Filters in AllFilterAppliedResultList on productData.SubCategory equals Filters
                        select new {
                            productData.Name,
                            productData.Product_Image,
                            productData.Area,
                            productData.Description,
                            productData.Length,
                            productData.Width,
                            productData.DisplayProductNumber,
                            productData.Model_ID

                        };

            foreach(var productData in query)
            {
                var ProductModel = ScriptableObject.CreateInstance<ProductsProperty>();
                ProductModel.Product_Name = productData.Name;
                ProductModel.Product_thumbnailUrl = productData.Product_Image;
                ProductModel.Product_Area = productData.Area;
                ProductModel.Product_description = productData.Description;
                ProductModel.product_Length = productData.Length;
                ProductModel.product_Width = productData.Width;
                ProductModel.DisplayProductNumber = productData.DisplayProductNumber;
                ProductModel.ModelID = productData.Model_ID;
                allProductResultList.Add(ProductModel);
            }

            /*foreach (GetAllProductResponse.ProductData productData in allProductsResultData.result)
            {
                foreach (string x in AllFilterAppliedResultList)
                {
                    if (x == productData.SubCategory)
                    {
                        var ProductModel = ScriptableObject.CreateInstance<ProductsProperty>();
                        ProductModel.Product_Name = productData.Name;
                        ProductModel.Product_thumbnailUrl = productData.Product_Image;
                        ProductModel.Product_Area = productData.Area;
                        ProductModel.Product_description = productData.Description;
                        ProductModel.product_Length = productData.Length;
                        ProductModel.product_Width = productData.Width;
                        ProductModel.DisplayProductNumber = productData.DisplayProductNumber;
                        ProductModel.ModelID = productData.Model_ID;
                        allProductResultList.Add(ProductModel);
                    }
                }
            }*/
        }

        /*foreach (GetAllProductResponse.ProductData productData in allProductsResultData.result)
        {
            if (AllFilterAppliedResultList.Count <= 0)
            {
                var ProductModel = ScriptableObject.CreateInstance<ProductsProperty>();
                ProductModel.Product_Name = productData.Name;
                ProductModel.Product_thumbnailUrl = productData.Product_Image;
                ProductModel.Product_Area = productData.Area;
                ProductModel.Product_description = productData.Description;
                ProductModel.product_Length = productData.Length;
                ProductModel.product_Width = productData.Width;
                ProductModel.DisplayProductNumber = productData.DisplayProductNumber;
                ProductModel.ModelID = productData.Model_ID;
                allProductResultList.Add(ProductModel);
                
            }
            else
            {
                foreach(string x in AllFilterAppliedResultList)
                {
                    if(x == productData.SubCategory)
                    {
                        var ProductModel = ScriptableObject.CreateInstance<ProductsProperty>();
                        ProductModel.Product_Name = productData.Name;
                        ProductModel.Product_thumbnailUrl = productData.Product_Image;
                        ProductModel.Product_Area = productData.Area;
                        ProductModel.Product_description = productData.Description;
                        ProductModel.product_Length = productData.Length;
                        ProductModel.product_Width = productData.Width;
                        ProductModel.DisplayProductNumber = productData.DisplayProductNumber;
                        ProductModel.ModelID = productData.Model_ID;
                        allProductResultList.Add(ProductModel);
                    }
                }
            }
        }*/

        E_GetAllproductONCatalog.Raise();

    }
    public void ApplyFilters()
    {
        GenerateProductResultList(allProductResult);
        /*else
        {
            foreach (string x in AllFilterAppliedResultList)
            {
                Debug.Log(x);
                GenerateProductResultList(allProductResult);
            }
        }*/
    }

    
}
