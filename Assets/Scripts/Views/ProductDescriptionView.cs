using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;
using System.Collections.Generic;
using UnityAtoms;
using TMPro;
using System;
using UnityEngine.UI;

public class ProductDescriptionView : MonoBehaviour
{
    [SerializeField]
    ObjectVariable V_CurrentSelectedItem;

    [SerializeField]
    StringEvent E_AddressableLoadAssetStart;


    [Header("GameObjects")]
    [SerializeField]
    TMP_Text productName;
    [SerializeField]
    TMP_Text productDescription;
    [SerializeField]
    TMP_Text Area;
    ProductsProperty _selectedProduct;
    [SerializeField]
    Image ThumbnailImage;
    
    [Space]
    [SerializeField]
    TMP_Text Count;
    private int itemCount = 1;

    [SerializeField]
    GameObject ProtipPanel;

    [SerializeField]
    StringVariable v_SpacesData;

    [SerializeField]
    IntVariable vi_CurrentLayout;

    [SerializeField]
    IntVariable vi_CurrentSpaces;

    [SerializeField]
    LayoutData layoutData;

    AllSpacesData allSpacesData;

    [SerializeField]
    Slider slider;

    [SerializeField]
    IntVariable v_sliderValue;

    [SerializeField]
    IntVariable V_SliderMaxValue;

    [SerializeField]
    StringEvent E_Toast;

    int area;


    // Start is called before the first frame update
    public void OnDisable()
    {
        gameObject.SetActive(false);
        
    }
    private void OnEnable()
    {
        
        gameObject.SetActive(true);

        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
        //layoutData = allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList[vi_CurrentLayout.Value];
        layoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));
        LoadProductInformation();
    }
    void LoadProductInformation()
    {
        //layoutData = new LayoutData();
        //layoutData = LayoutData.CreateFromJSON(v_SpacesData.Value);

        _selectedProduct = (ProductsProperty)V_CurrentSelectedItem.Value;
        Debug.Log(layoutData.productList.Count);

        Count.text = itemCount.ToString();
        area = layoutData.layoutLength * layoutData.layoutWidth;
        //      Slider.maxValue = area;
        

        productName.text = _selectedProduct.Product_Name;
        

        productDescription.text = _selectedProduct.Product_description;
        Area.text = String.Format("{0} X {1} ({2} ft\xB2)", _selectedProduct.product_Length, _selectedProduct.product_Width, _selectedProduct.Product_Area);
        
        LoadThumb(_selectedProduct.Product_thumbnailUrl);
        Debug.Log("Slider Max " + V_SliderMaxValue.Value);
        Debug.Log("Slider Value " + v_sliderValue.Value);
    }

    void LoadThumb(string thumbnailImageURL)
    {
        Davinci.get()
            .load(thumbnailImageURL)
            .withStartAction(() =>
            {
                Debug.Log("Loading");
            })
            .withDownloadedAction(() =>
            {
                Debug.Log("Downloaded ");
            })
            .withErrorAction((Error) =>
            {
                Debug.Log(Error);
            })
            .into(ThumbnailImage)
            .start();
    }

    public void AddItem()
    {
        itemCount++;
        Count.text = itemCount.ToString();

        /*if((int)_selectedProduct.Product_Area * itemCount<=area)
        {
            UpdateSlider();
        }*/
    }
    public void SubtractItem()
    {
        if(itemCount>1)
            itemCount--;
        Count.text = itemCount.ToString();
        //UpdateSlider();
    }
    /*public void UpdateSlider()
    {
        Debug.Log("Update Called");
        Slider.value = (int)_selectedProduct.Product_Area *itemCount;
        SliderValueText.text = Slider.value.ToString();
        Debug.Log(Slider.value);
    }*/
    public void AddToCart()
    {
        
        
        if (v_sliderValue.Value+((int)_selectedProduct.Product_Area*itemCount) >= V_SliderMaxValue.Value)
        {
            Debug.Log("item not Added");
            E_Toast.Raise("Space not Enough to Add More item");
        }
        else
        {
            Debug.Log("Adding Item");

            LayoutData.sku layoutSKU = new LayoutData.sku();
            //var layoutSKU = layoutData.productList;
            layoutSKU.productName = _selectedProduct.Product_Name;
            layoutSKU.ProductID = _selectedProduct.ModelID;
            layoutSKU.quantity = itemCount;
            layoutSKU.area = (int)_selectedProduct.Product_Area;
            layoutSKU.Thumbnail = _selectedProduct.Product_thumbnailUrl;
            layoutSKU.price = _selectedProduct.Price;

            bool AddNew = true;
            bool canAdd = false;

            for (int i = 0; i < layoutData.productList.Count; i++)
            {
                if (layoutData.productList[i].ProductID == _selectedProduct.ModelID)
                {
                    Debug.Log("Model Exists in List");
                    layoutData.productList[i].quantity += itemCount;
                    layoutData.productList[i].area = (int)_selectedProduct.Product_Area;
                    AddNew = false;
                    break;
                }

            }

            if (AddNew)
            {
                Debug.Log("Item not in cart");
                layoutData.productList.Add(layoutSKU);
                Debug.Log("Item  " + layoutSKU.quantity);

            }

            layoutData.SaveToString();
            
           
        }
        itemCount = 1;
        

    }

    

    public void LoadAssetForAR()
    {
        E_AddressableLoadAssetStart.Raise(_selectedProduct.ModelID);
    }

  

}
