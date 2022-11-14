using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;
using TMPro;

public class ShoppingCartPageView : MonoBehaviour
{
    [SerializeField]
    TMP_Text LayoutText;

    [SerializeField]
    StringVariable v_SelectedLayout;
        
    [SerializeField]
    StringVariable v_CurrentSelectedSpace;

    [SerializeField]
    LayoutData layoutData;
    [SerializeField]
    SpacesData hniSpacesData;

    [SerializeField]
    GameObject CartItemAdapter;
    [SerializeField]
    GameObject CartItemContainer;
    [SerializeField]
    List<GameObject> cartGameobjectList;

    [SerializeField]
    IntVariable vi_CurrentLayout;
    [SerializeField]
    IntVariable vi_CurrentSpaces;
   

    [SerializeField]
    AllSpacesData allSpacesData;

    public string SpaceName;

    public GameObject DescriptionPanel;

    public void OnEnable()
    {
        gameObject.SetActive(true);

        PopulateCartList();
        
        


    }
    public void OnDisable()
    {
       
        gameObject.SetActive(false);
       
        
    }
    public void saveLayout()
    {
        hniSpacesData = SpacesData.CreateFromJSON(PlayerPrefs.GetString("HNIspacesData"));
        hniSpacesData.LayoutDataList.Add(layoutData);
        //hniSpacesData.SaveToString();
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("AllSpaceData")))
        {
            allSpacesData.spacesDatas.Add(hniSpacesData);
            
            //allSpacesData.CheckandSave();
        }
        else
        {
            allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
            if (vi_CurrentSpaces.Value > allSpacesData.spacesDatas.Count)
            {
                allSpacesData.spacesDatas.Add(hniSpacesData);
            }
            else if (vi_CurrentLayout.Value > allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList.Count)
            {
                allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList.Add(layoutData);
            }            
            else
            {
                allSpacesData.UpdateLayout(vi_CurrentSpaces.Value, vi_CurrentLayout.Value, layoutData);
                //allSpacesData.CheckandSave();
            }

        }
        allSpacesData.SaveToString();
    }

    public void PopulateCartList()
    {
        
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));


        //layoutData = allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList[vi_CurrentLayout.Value];

        //PlayerPrefs.SetString("LayoutData", layoutData.SaveToString());

        foreach (Transform obj in CartItemContainer.transform)
            Destroy(obj.gameObject);
        cartGameobjectList.Clear();

        var cartData = allSpacesData.spacesDatas;

        layoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));
        LayoutText.text = layoutData.layoutName;

        //SpaceName = allSpacesData.spacesDatas[vi_CurrentSpaces.Value].spaceName;

        var CartItems = layoutData.productList.Count;
        Debug.Log("Populating Cart");
        for (var x = 0; x < CartItems; x++)
        {
            Debug.Log(layoutData.productList[x].quantity);
            GameObject CartItem = Instantiate(CartItemAdapter, CartItemContainer.transform);
            cartGameobjectList.Add(CartItem);
            CartItem.GetComponent<cartListAdapterView>().SetCartData(layoutData.productList[x].productName, layoutData.productList[x].area, layoutData.productList[x].quantity, layoutData.productList[x].Thumbnail,x);
        }
    }

    public void RemoveFromCart(int val)
    {
        layoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));
        layoutData.productList.RemoveAt(val);
        layoutData.SaveToString();
        saveLayout();
        //Dont Touch These Lines
        PopulateCartList();
    }

    


}
