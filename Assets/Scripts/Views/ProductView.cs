using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.UI;
using TMPro;

public class ProductView : MonoBehaviour
{
    [SerializeField]
    GameObject ProductAdapter;
    [SerializeField]
    GameObject ProductContainer;
    [SerializeField]
    GameObject SliderContainer;
    [Space]
    [SerializeField]
    Slider mainAreaSlider;
    [SerializeField]
    TMP_Text MaxValue;
    [SerializeField]
    TMP_Text currentValue;
    [Space]
    [SerializeField]
    ObjectValueList allProductResultList;
    [SerializeField]
    List<GameObject> productGameobjectList;
    

    [SerializeField]
    StringVariable v_SpacesData;

    [SerializeField]
    IntVariable vi_CurrentSpaces;

    [SerializeField]
    IntVariable vi_CurrentLayout;

    [SerializeField]
    LayoutData layoutData;

    [SerializeField]
    SpacesData spacesData;

    AllSpacesData allSpacesData;
    [Header("SliderEvents")]
    [SerializeField]
    IntEvent E_MaxValue;
    [SerializeField]
    IntEvent E_Value;
    [SerializeField]
    IntVariable v_sliderMaxValue;

    public void LoadProductData()
    {
        foreach (Transform obj in ProductContainer.transform)
            Destroy(obj.gameObject);
        productGameobjectList.Clear();  

        foreach (ProductsProperty products in allProductResultList)
        {
            GameObject ProductItem = Instantiate(ProductAdapter,  ProductContainer.transform);
            productGameobjectList.Add(ProductItem);
            ProductItem.GetComponent<ProductAdapterView>().UpdateProductView(products);

        }
    }

    private void OnEnable()
    {
        Debug.Log(vi_CurrentSpaces.Value);

        
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("AllSpaceData")))
        {
            allSpacesData = new AllSpacesData();
            //layoutData = new LayoutData();
            layoutData = LayoutData.CreateFromJSON(v_SpacesData.Value);
        }
        else
        {
            allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));

            if (vi_CurrentSpaces.Value > allSpacesData.spacesDatas.Count)
            {
                spacesData = SpacesData.CreateFromJSON(PlayerPrefs.GetString("HNIspacesData"));
                layoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));
                spacesData.LayoutDataList.Add(layoutData);
            }

            else if (vi_CurrentLayout.Value > allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList.Count)
                layoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));

            else
            {
                layoutData = allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList[vi_CurrentLayout.Value];
            }
        }
            

        layoutData.SaveToString();
        allSpacesData.SaveToString();
        
        int area = layoutData.layoutLength * layoutData.layoutWidth;
        E_MaxValue.Raise(area);

        //v_sliderMaxValue.Value = area;


        //SliderContainer.GetComponent<SliderContainerView>().SetSize(area);
        //Debug.Log(area);*/
    }
}
